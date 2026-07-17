namespace ECommerce.Application.Contracts.Infrastructure;

public class StockItemDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int TotalQuantity { get; set; }
    public int ReservedQuantity { get; set; }
    public int AvailableQuantity { get; set; }
}

public class ReserveItemDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

public class ReserveResponseDto
{
    public bool Reserved { get; set; }
    public List<ReserveItemResultDto> Items { get; set; } = [];
}

public class ReserveItemResultDto
{
    public Guid ProductId { get; set; }
    public bool Reserved { get; set; }
    public string? Reason { get; set; }
}

public interface IStockServiceClient
{
    Task<List<StockItemDto>> GetAllStockAsync(CancellationToken ct = default);
    Task<StockItemDto?> GetStockAsync(Guid productId, CancellationToken ct = default);
    Task<ReserveResponseDto> ReserveAsync(List<ReserveItemDto> items, CancellationToken ct = default);
}
