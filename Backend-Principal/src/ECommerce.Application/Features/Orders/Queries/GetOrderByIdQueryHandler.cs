using ECommerce.Domain.Contracts.Persistence;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions;
using MediatR;

namespace ECommerce.Application.Features.Orders.Queries;

public class GetOrderByIdQueryHandler(IOrderRepository orderRepository)
    : IRequestHandler<GetOrderByIdQuery, OrderDto>
{
    public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken ct)
    {
        var order = await orderRepository.GetByIdWithItemsAsync(request.OrderId, ct);
        if (order is null)
            throw new NotFoundException(nameof(Order), request.OrderId);

        if (!request.IsAdmin && order.UserId != request.RequestingUserId)
            throw new UnauthorizedAccessException("No tienes permiso para ver esta orden.");

        return new OrderDto(
            order.Id, order.UserId, order.CreatedAt, order.Status.ToString(), order.Total,
            order.Items.Select(i => new OrderItemDto(i.Id, i.ProductId, i.UnitPrice, i.Quantity, i.Subtotal)).ToList());
    }
}
