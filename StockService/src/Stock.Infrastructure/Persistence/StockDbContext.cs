using Stock.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Stock.Infrastructure.Persistence;

public class StockDbContext : DbContext
{
    public StockDbContext(DbContextOptions<StockDbContext> options)
        : base(options) { }

    public DbSet<ProductStock> ProductStocks { get; set; } = null!;
    public DbSet<StockMovement> StockMovements { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(StockDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
