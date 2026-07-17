using Stock.Domain.Contracts.Persistence;
using Stock.Application.Contracts.Infrastructure;
using Stock.Infrastructure.Persistence;
using Stock.Infrastructure.Repositories;
using Stock.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Stock.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<StockDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IProductStockRepository, ProductStockRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddHttpClient<IProductoServiceClient, ProductoServiceClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["EcommerceApi:BaseUrl"] ?? "http://localhost:5047");
            client.Timeout = TimeSpan.FromSeconds(5);
        });

        return services;
    }
}
