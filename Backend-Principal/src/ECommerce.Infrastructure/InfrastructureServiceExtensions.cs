using ECommerce.Domain.Contracts.Persistence;
using ECommerce.Application.Contracts.Infrastructure;
using ECommerce.Infrastructure.Persistence;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository,  ProductRepository>();
        services.AddScoped<IOrderRepository,    OrderRepository>();
        services.AddScoped<IUserRepository,     UserRepository>();
        services.AddScoped<ITokenService,       TokenGenerator>();
        services.AddScoped<IHashService,        PasswordHasher>();
        services.AddScoped<IUnitOfWork,         UnitOfWork>();

        services.AddHttpClient<IStockServiceClient, StockServiceClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["StockService:BaseUrl"] ?? "http://localhost:5050");
            client.Timeout = TimeSpan.FromSeconds(5);
        });

        return services;
    }
}
