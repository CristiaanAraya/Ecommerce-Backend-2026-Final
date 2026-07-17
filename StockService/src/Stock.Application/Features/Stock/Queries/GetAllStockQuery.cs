using MediatR;

namespace Stock.Application.Features.Stock.Queries;

public record GetAllStockQuery : IRequest<List<StockDto>>;
