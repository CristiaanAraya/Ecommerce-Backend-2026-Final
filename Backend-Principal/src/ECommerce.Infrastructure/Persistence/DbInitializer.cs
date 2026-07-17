using ECommerce.Domain.Contracts.Persistence;
using ECommerce.Application.Contracts.Infrastructure;
using ECommerce.Domain.Entities;
using ECommerce.Domain.ValueObjects;
using ECommerce.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure.Persistence;

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

    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var hashService = scope.ServiceProvider.GetRequiredService<IHashService>();

        await context.Database.MigrateAsync();

        const string adminEmailStr = "admin@ecommerce.com";
        if (!context.Users.AsEnumerable().Any(u => u.Email.Value == adminEmailStr))
        {
            var admin = new User(
                "admin@ecommerce.com",
                "Administrador",
                hashService.ComputeHash("Admin@2024"),
                "Admin");
            await context.Users.AddAsync(admin);
        }

        const string clientEmailStr = "cliente@ecommerce.com";
        if (!context.Users.AsEnumerable().Any(u => u.Email.Value == clientEmailStr))
        {
            var customer = new User(
                "cliente@ecommerce.com",
                "Cliente Demo",
                hashService.ComputeHash("Cliente@2024"),
                "Customer");
            await context.Users.AddAsync(customer);
        }

        if (!await context.Products.AnyAsync())
        {
            var products = new[]
            {
                Product.New("Laptop Gamer",         "Laptop de alto rendimiento para gaming",               1500000m, 10, CategoryConfiguration.ElectronicaId, LaptopGamerId),
                Product.New("Teclado Mecánico",     "Teclado mecánico RGB con switches Cherry MX",          85000m,   50, CategoryConfiguration.ElectronicaId, TecladoMecanicoId),
                Product.New("Mouse Inalámbrico",    "Mouse inalámbrico ergonómico con 6 botones",           45000m,   30, CategoryConfiguration.ElectronicaId, MouseInalambricoId),
                Product.New("Monitor 27\" 4K",      "Monitor IPS 27 pulgadas resolución 4K",                650000m,  15, CategoryConfiguration.ElectronicaId, Monitor4KId),
                Product.New("Auriculares Bluetooth","Auriculares inalámbricos con cancelación de ruido",    55000m,   25, CategoryConfiguration.ElectronicaId, AuricularesBTId),
                Product.New("Smartwatch Deportivo", "Reloj inteligente con GPS y monitor cardíaco",         120000m,  20, CategoryConfiguration.ElectronicaId, SmartwatchId),
                Product.New("Parlante Portátil",    "Parlante Bluetooth portátil resistente al agua",        35000m,   40, CategoryConfiguration.ElectronicaId, ParlanteId),
                Product.New("Disco SSD 1TB",        "Unidad de estado sólido NVMe 1TB",                     180000m,  35, CategoryConfiguration.ElectronicaId, DiscoSSDId),
                Product.New("Tablet 10\"",          "Tablet con pantalla de 10 pulgadas y 128GB",           250000m,  12, CategoryConfiguration.HogarId, TabletId),
                Product.New("Cargador Inalámbrico", "Cargador inalámbrico rápido Qi compatible",             25000m,   60, CategoryConfiguration.ElectronicaId, CargadorId)
            };
            await context.Products.AddRangeAsync(products);
        }

        await context.SaveChangesAsync();
    }
}
