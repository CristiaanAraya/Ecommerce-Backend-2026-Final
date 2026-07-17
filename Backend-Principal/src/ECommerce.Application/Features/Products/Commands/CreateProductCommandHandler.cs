using ECommerce.Domain.Contracts.Persistence;
using ECommerce.Application.Features.Products.Queries;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions;
using MediatR;

namespace ECommerce.Application.Features.Products.Commands;

public class CreateProductCommandHandler(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken ct)
    {
        var category = await categoryRepository.GetByIdAsync(request.CategoryId, ct);
        if (category is null)
            throw new NotFoundException(nameof(Category), request.CategoryId);

        var product = Product.New(request.Name, request.Description, request.Price, request.Stock, request.CategoryId);
        await productRepository.AddAsync(product, ct);
        await unitOfWork.SaveChangesAsync(ct);

        var saved = await productRepository.GetByIdAsync(product.Id, ct) ?? product;
        return new ProductDto(
            saved.Id, saved.Name, saved.Description, saved.Price, saved.Stock,
            saved.CategoryId, saved.Category?.Name ?? category.Name, saved.IsActive);
    }
}
