using MediatR;

namespace ECommerce.Application.Features.Products.Queries;

public record GetAllProductsQuery() : IRequest<IEnumerable<ProductDto>>;
