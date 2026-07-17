using ECommerce.Domain.Contracts.Persistence;
using MediatR;

namespace ECommerce.Application.Features.Categories.Queries;

public class GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>>
{
    public async Task<IEnumerable<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken ct)
    {
        var categories = await categoryRepository.GetAllAsync(ct);
        return categories.Select(c => new CategoryDto(c.Id, c.Name));
    }
}
