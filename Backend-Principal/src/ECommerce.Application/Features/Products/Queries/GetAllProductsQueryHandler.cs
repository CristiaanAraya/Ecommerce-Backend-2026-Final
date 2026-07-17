using ECommerce.Domain.Contracts.Persistence;
using MediatR;

namespace ECommerce.Application.Features.Products.Queries;

public class GetAllProductsQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
{
    public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken ct)
    {
        var products = await productRepository.GetAllAsync(ct);
        return products.Select(p => new ProductDto(
            p.Id, p.Name, p.Description, p.Price, p.Stock,
            p.CategoryId, p.Category?.Name ?? string.Empty, p.IsActive));
    }
}
