using ECommerce.Application.Contracts.Infrastructure;
using ECommerce.Domain.Contracts.Persistence;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions;
using MediatR;

namespace ECommerce.Application.Features.Orders.Commands;

public class PlaceOrderCommandHandler(
    IUserRepository userRepository,
    IProductRepository productRepository,
    IOrderRepository orderRepository,
    IStockServiceClient stockService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<PlaceOrderCommand, PlaceOrderResponse>
{
    public async Task<PlaceOrderResponse> Handle(PlaceOrderCommand request, CancellationToken ct)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, ct);
        if (user is null)
            throw new NotFoundException(nameof(User), request.UserId);

        if (request.Lines.Count == 0)
            throw new BusinessException("La orden debe contener al menos un producto.");

        var reserveItems = request.Lines
            .Select(l => new ReserveItemDto { ProductId = l.ProductId, Quantity = l.Quantity })
            .ToList();

        var reserveResult = await stockService.ReserveAsync(reserveItems, ct);
        if (!reserveResult.Reserved)
        {
            var failures = reserveResult.Items
                .Where(i => !i.Reserved)
                .Select(i => $"- Producto {i.ProductId}: {i.Reason}");

            throw new BusinessException(
                $"No se pudo reservar stock para todos los productos:\n{string.Join("\n", failures)}");
        }

        var order = new Order(request.UserId);

        foreach (var line in request.Lines)
        {
            var product = await productRepository.GetByIdAsync(line.ProductId, ct);
            if (product is null)
                throw new NotFoundException(nameof(Product), line.ProductId);

            order.AddItem(product, line.Quantity);
        }

        await orderRepository.AddAsync(order, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return new PlaceOrderResponse(order.Id, order.UserId, order.CreatedAt, order.Status.ToString(), order.Total);
    }
}
