using ECommerce.Domain.Contracts.Persistence;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence;
using ECommerce.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext ctx) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await ctx.Users.FindAsync(new object[] { id }, ct);

    public Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        var normalizedEmail = email.Trim().ToLower();
        var user = ctx.Users
            .AsNoTracking()
            .AsEnumerable()
            .FirstOrDefault(u => u.Email.Value == normalizedEmail);
        return Task.FromResult(user);
    }

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default)
    {
        var normalizedEmail = email.Trim().ToLower();
        var exists = ctx.Users.AsEnumerable().Any(u => u.Email.Value == normalizedEmail);
        return Task.FromResult(exists);
    }

    public async Task AddAsync(User user, CancellationToken ct = default)
        => await ctx.Users.AddAsync(user, ct);
}
