using Stock.Domain.Contracts.Persistence;
using MediatR;

namespace Stock.Application.Features.Stock.Commands;

public class ReleaseStockCommandHandler(
    IProductStockRepository productStockRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ReleaseStockCommand, ReleaseStockResponse>
{
    public async Task<ReleaseStockResponse> Handle(ReleaseStockCommand request, CancellationToken ct)
    {
        var response = new ReleaseStockResponse();

        foreach (var item in request.Items)
        {
            var stock = await productStockRepository.GetByProductIdAsync(item.ProductId, ct);

            if (stock is null)
            {
                response.Items.Add(new ReleaseItemResult
                {
                    ProductId = item.ProductId,
                    Released = false,
                    Reason = $"Producto con id '{item.ProductId}' no tiene stock registrado."
                });
                continue;
            }

            if (item.Quantity > stock.ReservedQuantity)
            {
                response.Items.Add(new ReleaseItemResult
                {
                    ProductId = item.ProductId,
                    Released = false,
                    Reason = $"No se pueden liberar {item.Quantity} unidades. Solo {stock.ReservedQuantity} están reservadas."
                });
                continue;
            }

            stock.Release(item.Quantity);
            await productStockRepository.UpdateAsync(stock, ct);

            response.Items.Add(new ReleaseItemResult
            {
                ProductId = item.ProductId,
                Released = true
            });
        }

        if (response.Items.Count > 0)
        {
            await unitOfWork.SaveChangesAsync(ct);
        }

        response.Released = response.Items.TrueForAll(i => i.Released);

        return response;
    }
}
