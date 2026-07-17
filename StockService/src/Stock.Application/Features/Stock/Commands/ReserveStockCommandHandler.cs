using Stock.Domain.Contracts.Persistence;
using MediatR;

namespace Stock.Application.Features.Stock.Commands;

public class ReserveStockCommandHandler(
    IProductStockRepository productStockRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ReserveStockCommand, ReserveStockResponse>
{
    public async Task<ReserveStockResponse> Handle(ReserveStockCommand request, CancellationToken ct)
    {
        var response = new ReserveStockResponse();

        foreach (var item in request.Items)
        {
            var stock = await productStockRepository.GetByProductIdAsync(item.ProductId, ct);

            if (stock is null)
            {
                response.Items.Add(new ReserveItemResult
                {
                    ProductId = item.ProductId,
                    Reserved = false,
                    Reason = $"Producto con id '{item.ProductId}' no tiene stock registrado."
                });
                continue;
            }

            if (!stock.CanReserve(item.Quantity))
            {
                response.Items.Add(new ReserveItemResult
                {
                    ProductId = item.ProductId,
                    Reserved = false,
                    Reason = $"Stock insuficiente. Disponible: {stock.AvailableQuantity}, solicitado: {item.Quantity}."
                });
                continue;
            }

            stock.Reserve(item.Quantity);
            await productStockRepository.UpdateAsync(stock, ct);

            response.Items.Add(new ReserveItemResult
            {
                ProductId = item.ProductId,
                Reserved = true
            });
        }

        if (response.Items.Count > 0)
        {
            await unitOfWork.SaveChangesAsync(ct);
        }

        response.Reserved = response.Items.TrueForAll(i => i.Reserved);

        return response;
    }
}
