using Stock.Domain.Contracts.Persistence;
using Stock.Domain.Entities;
using Stock.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Stock.Infrastructure.Repositories;

public class ProductStockRepository(StockDbContext ctx) : IProductStockRepository
{
    public async Task<ProductStock?> GetByProductIdAsync(Guid productId, CancellationToken ct = default)
        => await ctx.ProductStocks
            .FirstOrDefaultAsync(p => p.ProductId == productId, ct);

    public async Task<List<ProductStock>> GetAllAsync(CancellationToken ct = default)
        => await ctx.ProductStocks
            .OrderBy(p => p.ProductName)
            .ToListAsync(ct);

    public async Task AddAsync(ProductStock productStock, CancellationToken ct = default)
        => await ctx.ProductStocks.AddAsync(productStock, ct);

    public Task UpdateAsync(ProductStock productStock, CancellationToken ct = default)
    {
        ctx.ProductStocks.Update(productStock);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(Guid productId, CancellationToken ct = default)
        => await ctx.ProductStocks.AnyAsync(p => p.ProductId == productId, ct);
}
