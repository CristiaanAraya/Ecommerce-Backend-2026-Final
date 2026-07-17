using Stock.Domain.Contracts.Persistence;

namespace Stock.Infrastructure.Persistence;

public class UnitOfWork(StockDbContext ctx) : IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken ct = default) => ctx.SaveChangesAsync(ct);
}
