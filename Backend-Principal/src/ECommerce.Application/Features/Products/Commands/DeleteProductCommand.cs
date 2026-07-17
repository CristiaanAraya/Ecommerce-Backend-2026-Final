using MediatR;

namespace ECommerce.Application.Features.Products.Commands;

public record DeleteProductCommand(Guid Id) : IRequest<Unit>;
