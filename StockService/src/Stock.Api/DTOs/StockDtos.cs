namespace Stock.Api.DTOs;

public record ReserveStockRequest
{
    public List<ReserveItemRequest> Items { get; set; } = [];
}

public record ReserveItemRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

public record ReleaseStockRequest
{
    public List<ReleaseItemRequest> Items { get; set; } = [];
}

public record ReleaseItemRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
