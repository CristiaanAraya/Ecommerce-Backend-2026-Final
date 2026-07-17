using MediatR;

namespace Stock.Application.Features.Stock.Commands;

public class ReleaseStockCommand : IRequest<ReleaseStockResponse>
{
    public List<ReleaseItem> Items { get; set; } = [];
}

public class ReleaseItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

public class ReleaseStockResponse
{
    public bool Released { get; set; }
    public List<ReleaseItemResult> Items { get; set; } = [];
}

public class ReleaseItemResult
{
    public Guid ProductId { get; set; }
    public bool Released { get; set; }
    public string? Reason { get; set; }
}
