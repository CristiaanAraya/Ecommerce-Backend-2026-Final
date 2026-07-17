using ECommerce.Domain.Contracts.Persistence;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions;
using MediatR;

namespace ECommerce.Application.Features.Products.Queries;

public class GetProductByIdQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken ct)
    {
        var product = await productRepository.GetByIdAsync(request.Id, ct);
        if (product is null)
            throw new NotFoundException(nameof(Product), request.Id);

        return new ProductDto(
            product.Id, product.Name, product.Description, product.Price, product.Stock,
            product.CategoryId, product.Category?.Name ?? string.Empty, product.IsActive);
    }
}
