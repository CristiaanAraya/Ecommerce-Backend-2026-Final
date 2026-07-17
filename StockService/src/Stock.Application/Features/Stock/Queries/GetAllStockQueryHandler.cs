using Stock.Domain.Contracts.Persistence;
using Stock.Domain.Entities;
using MediatR;

namespace Stock.Application.Features.Stock.Queries;

public class GetAllStockQueryHandler(
    IProductStockRepository productStockRepository)
    : IRequestHandler<GetAllStockQuery, List<StockDto>>
{
    public async Task<List<StockDto>> Handle(GetAllStockQuery request, CancellationToken ct)
    {
        var stocks = await productStockRepository.GetAllAsync(ct);

        return stocks.Select(s => new StockDto(
            s.ProductId, s.ProductName, s.TotalQuantity, s.ReservedQuantity, s.AvailableQuantity)).ToList();
    }
}
