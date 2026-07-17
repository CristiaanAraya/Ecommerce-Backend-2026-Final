using MediatR;

namespace Stock.Application.Features.Stock.Commands;

public class ReserveStockCommand : IRequest<ReserveStockResponse>
{
    public List<ReserveItem> Items { get; set; } = [];
}

public class ReserveItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

public class ReserveStockResponse
{
    public bool Reserved { get; set; }
    public List<ReserveItemResult> Items { get; set; } = [];
}

public class ReserveItemResult
{
    public Guid ProductId { get; set; }
    public bool Reserved { get; set; }
    public string? Reason { get; set; }
}
