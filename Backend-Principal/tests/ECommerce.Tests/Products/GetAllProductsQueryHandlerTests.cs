using ECommerce.Domain.Contracts.Persistence;
using ECommerce.Application.Contracts.Infrastructure;
using ECommerce.Application.Features.Products.Queries;
using ECommerce.Domain.Entities;
using Moq;

namespace ECommerce.Tests.Products;

public class GetAllProductsQueryHandlerTests
{
    private readonly Mock<IProductRepository> _repoMock = new();
    private readonly GetAllProductsQueryHandler _handler;

    public GetAllProductsQueryHandlerTests()
    {
        _handler = new GetAllProductsQueryHandler(_repoMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsAllProducts()
    {
        var products = new List<Product>
        {
            Product.New("Laptop", "Descripción", 1500m, 10, Guid.NewGuid()),
            Product.New("Mouse", "Descripción", 30m, 50, Guid.NewGuid())
        };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(products);

        var result = await _handler.Handle(new GetAllProductsQuery(), default);

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenNoProducts()
    {
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync([]);

        var result = await _handler.Handle(new GetAllProductsQuery(), default);

        Assert.Empty(result);
    }
}
