using ECommerce.Domain.Contracts.Persistence;
using ECommerce.Domain.Exceptions;
using MediatR;

namespace ECommerce.Application.Features.Products.Queries;

public class SearchProductsQueryHandler(IProductRepository productRepository)
    : IRequestHandler<SearchProductsQuery, IEnumerable<ProductDto>>
{
    public async Task<IEnumerable<ProductDto>> Handle(SearchProductsQuery request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Term))
            throw new BusinessException("El término de búsqueda es requerido.");

        var products = await productRepository.SearchByNameAsync(request.Term, ct);
        return products.Select(p => new ProductDto(
            p.Id, p.Name, p.Description, p.Price, p.Stock,
            p.CategoryId, p.Category?.Name ?? string.Empty, p.IsActive));
    }
}
