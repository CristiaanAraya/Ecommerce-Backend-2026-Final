namespace Stock.Application.Contracts.Infrastructure;

public class ProductInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

public interface IProductoServiceClient
{
    Task<ProductInfo?> GetProductByIdAsync(Guid productId, CancellationToken ct = default);
}
