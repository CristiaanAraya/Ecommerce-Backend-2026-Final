using Stock.Domain.Entities;

namespace Stock.Domain.Contracts.Persistence;

public interface IProductStockRepository
{
    Task<ProductStock?> GetByProductIdAsync(Guid productId, CancellationToken ct = default);
    Task<List<ProductStock>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(ProductStock productStock, CancellationToken ct = default);
    Task UpdateAsync(ProductStock productStock, CancellationToken ct = default);
    Task<bool> ExistsAsync(Guid productId, CancellationToken ct = default);
}
