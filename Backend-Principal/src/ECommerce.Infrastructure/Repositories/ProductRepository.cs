using ECommerce.Domain.Common;
using ECommerce.Domain.Contracts.Persistence;
using ECommerce.Application.Contracts.Infrastructure;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories;

public class ProductRepository(ApplicationDbContext ctx) : IProductRepository
{
    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await ctx.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct = default)
        => await ctx.Products
            .Include(p => p.Category)
            .Where(p => p.IsActive)
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<IEnumerable<Product>> SearchByNameAsync(string term, CancellationToken ct = default)
        => await ctx.Products
            .Include(p => p.Category)
            .Where(p => p.Name.Contains(term) && p.IsActive)
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<PagedData<Product>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var query = ctx.Products
            .Include(p => p.Category)
            .Where(p => p.IsActive)
            .AsNoTracking();

        var total = await query.CountAsync(ct);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedData<Product>
        {
            Items = items,
            TotalCount = total,
            CurrentPage = page,
            PageSize = pageSize
        };
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
        => await ctx.Products.AnyAsync(p => p.Id == id, ct);

    public async Task AddAsync(Product product, CancellationToken ct = default)
        => await ctx.Products.AddAsync(product, ct);

    public Task UpdateAsync(Product product, CancellationToken ct = default)
    {
        ctx.Products.Update(product);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var product = await ctx.Products.FindAsync(new object[] { id }, ct);
        product?.Deactivate();
    }
}
