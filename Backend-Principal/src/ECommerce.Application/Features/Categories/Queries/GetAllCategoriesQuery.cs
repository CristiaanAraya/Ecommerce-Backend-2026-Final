using MediatR;

namespace ECommerce.Application.Features.Categories.Queries;

public record GetAllCategoriesQuery() : IRequest<IEnumerable<CategoryDto>>;
