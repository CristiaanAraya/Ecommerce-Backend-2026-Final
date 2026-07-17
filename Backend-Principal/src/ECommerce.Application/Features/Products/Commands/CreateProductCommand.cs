using ECommerce.Application.Features.Products.Queries;
using MediatR;

namespace ECommerce.Application.Features.Products.Commands;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int Stock,
    Guid CategoryId) : IRequest<ProductDto>;
