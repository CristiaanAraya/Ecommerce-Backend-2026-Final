using ECommerce.Domain.Contracts.Persistence;
using ECommerce.Application.Contracts.Infrastructure;

namespace ECommerce.Infrastructure.Persistence;

public class UnitOfWork(ApplicationDbContext ctx) : IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken ct = default) => ctx.SaveChangesAsync(ct);
}
