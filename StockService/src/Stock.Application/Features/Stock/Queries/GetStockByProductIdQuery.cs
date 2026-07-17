using MediatR;

namespace Stock.Application.Features.Stock.Queries;

public record GetStockByProductIdQuery(Guid ProductId) : IRequest<StockDto>;
