using Stock.Domain.Contracts.Persistence;
using Stock.Domain.Entities;
using Stock.Domain.Exceptions;
using MediatR;

namespace Stock.Application.Features.Stock.Queries;

public class GetStockByProductIdQueryHandler(
    IProductStockRepository productStockRepository)
    : IRequestHandler<GetStockByProductIdQuery, StockDto>
{
    public async Task<StockDto> Handle(GetStockByProductIdQuery request, CancellationToken ct)
    {
        var stock = await productStockRepository.GetByProductIdAsync(request.ProductId, ct);
        if (stock is null)
            throw new NotFoundException(nameof(ProductStock), request.ProductId);

        return MapToDto(stock);
    }

    private static StockDto MapToDto(ProductStock s) =>
        new(s.ProductId, s.ProductName, s.TotalQuantity, s.ReservedQuantity, s.AvailableQuantity);
}
