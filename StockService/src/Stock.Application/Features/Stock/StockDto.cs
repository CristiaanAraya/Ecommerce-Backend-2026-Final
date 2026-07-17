namespace Stock.Application.Features.Stock;

public record StockDto(
    Guid ProductId,
    string ProductName,
    int TotalQuantity,
    int ReservedQuantity,
    int AvailableQuantity);
