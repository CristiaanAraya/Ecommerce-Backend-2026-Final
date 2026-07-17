using MediatR;

namespace ECommerce.Application.Features.Orders.Queries;

public record GetOrdersByUserQuery(Guid UserId) : IRequest<IEnumerable<OrderDto>>;
