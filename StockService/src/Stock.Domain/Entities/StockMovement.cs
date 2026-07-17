namespace Stock.Domain.Entities;

public enum StockChangeType { Addition, Reservation, Release }

public class StockMovement
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public StockChangeType ChangeType { get; private set; }
    public int Quantity { get; private set; }
    public int PreviousTotal { get; private set; }
    public int NewTotal { get; private set; }
    public string? Reason { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public ProductStock? Product { get; private set; }

    private StockMovement() { }

    public StockMovement(Guid productId, StockChangeType changeType, int quantity, int previousTotal, string? reason = null)
    {
        Id = Guid.NewGuid();
        ProductId = productId;
        ChangeType = changeType;
        Quantity = quantity;
        PreviousTotal = previousTotal;
        NewTotal = changeType switch
        {
            StockChangeType.Addition => previousTotal + quantity,
            StockChangeType.Reservation => previousTotal,
            StockChangeType.Release => previousTotal,
            _ => previousTotal
        };
        Reason = reason;
        CreatedAt = DateTime.UtcNow;
    }
}
