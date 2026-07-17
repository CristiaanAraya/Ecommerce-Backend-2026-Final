using Stock.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Stock.Infrastructure.Persistence;

public static class DbInitializer
{
    public static readonly Guid LaptopGamerId      = Guid.Parse("00000000-0000-0000-0000-000000000001");
    public static readonly Guid TecladoMecanicoId  = Guid.Parse("00000000-0000-0000-0000-000000000002");
    public static readonly Guid MouseInalambricoId = Guid.Parse("00000000-0000-0000-0000-000000000003");
    public static readonly Guid Monitor4KId        = Guid.Parse("00000000-0000-0000-0000-000000000004");
    public static readonly Guid AuricularesBTId    = Guid.Parse("00000000-0000-0000-0000-000000000005");
    public static readonly Guid SmartwatchId       = Guid.Parse("00000000-0000-0000-0000-000000000006");
    public static readonly Guid ParlanteId         = Guid.Parse("00000000-0000-0000-0000-000000000007");
    public static readonly Guid DiscoSSDId         = Guid.Parse("00000000-0000-0000-0000-000000000008");
    public static readonly Guid TabletId           = Guid.Parse("00000000-0000-0000-0000-000000000009");
    public static readonly Guid CargadorId         = Guid.Parse("00000000-0000-0000-0000-00000000000a");

    public static async Task InitializeAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<StockDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
            .CreateLogger(nameof(DbInitializer));

        await context.Database.EnsureCreatedAsync();

        if (!await context.ProductStocks.AnyAsync())
        {
            logger.LogInformation("Seeding initial stock data...");

            context.ProductStocks.AddRange(
                new ProductStock(LaptopGamerId,      "Laptop Gamer",         10),
                new ProductStock(TecladoMecanicoId,  "Teclado Mecánico",     50),
                new ProductStock(MouseInalambricoId, "Mouse Inalámbrico",    30),
                new ProductStock(Monitor4KId,        "Monitor 27\" 4K",      15),
                new ProductStock(AuricularesBTId,    "Auriculares Bluetooth",25),
                new ProductStock(SmartwatchId,       "Smartwatch Deportivo", 20),
                new ProductStock(ParlanteId,         "Parlante Portátil",    40),
                new ProductStock(DiscoSSDId,         "Disco SSD 1TB",        35),
                new ProductStock(TabletId,           "Tablet 10\"",          12),
                new ProductStock(CargadorId,         "Cargador Inalámbrico", 60)
            );

            await context.SaveChangesAsync();
            logger.LogInformation("Seed data inserted successfully.");
        }
    }
}
