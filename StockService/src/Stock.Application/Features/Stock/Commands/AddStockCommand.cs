using MediatR;

namespace Stock.Application.Features.Stock.Commands;

public record AddStockCommand(Guid ProductId, int Quantity) : IRequest<StockDto>;
