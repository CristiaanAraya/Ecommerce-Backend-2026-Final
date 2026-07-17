using ECommerce.Domain.Common;
using ECommerce.Domain.Contracts.Persistence;
using MediatR;

namespace ECommerce.Application.Features.Products.Queries;

public class GetPagedProductsQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetPagedProductsQuery, PagedData<ProductDto>>
{
    public async Task<PagedData<ProductDto>> Handle(GetPagedProductsQuery request, CancellationToken ct)
    {
        var paged = await productRepository.GetPagedAsync(request.Page, request.PageSize, ct);
        return new PagedData<ProductDto>
        {
            Items = paged.Items.Select(p => new ProductDto(
                p.Id, p.Name, p.Description, p.Price, p.Stock,
                p.CategoryId, p.Category?.Name ?? string.Empty, p.IsActive)).ToList(),
            TotalCount = paged.TotalCount,
            CurrentPage = paged.CurrentPage,
            PageSize = paged.PageSize
        };
    }
}
