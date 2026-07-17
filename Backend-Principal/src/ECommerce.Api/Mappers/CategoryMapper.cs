using ECommerce.Api.DTOs;
using ECommerce.Domain.Entities;

namespace ECommerce.Api.Mappers;

public static class CategoryMapper
{
    public static CategoryResponse ToResponse(Category category) =>
        new(category.Id, category.Name);
}
