using ECommerce.Api.DTOs;
using ECommerce.Domain.Common;
using ECommerce.Domain.Entities;

namespace ECommerce.Api.Mappers;

public static class ProductMapper
{
    public static ProductResponse ToResponse(Product product) =>
        new(product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.Stock,
            product.CategoryId,
            product.Category?.Name ?? string.Empty,
            product.IsActive);

    public static PagedResult<ProductResponse> ToPagedResult(PagedData<Product> data) =>
        new()
        {
            Items = data.Items.Select(ToResponse).ToList(),
            TotalCount = data.TotalCount,
            TotalPages = data.TotalPages,
            CurrentPage = data.CurrentPage,
            PageSize = data.PageSize
        };
}
