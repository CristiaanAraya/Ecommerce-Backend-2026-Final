using MediatR;

namespace ECommerce.Application.Features.Products.Queries;

public record SearchProductsQuery(string Term) : IRequest<IEnumerable<ProductDto>>;
