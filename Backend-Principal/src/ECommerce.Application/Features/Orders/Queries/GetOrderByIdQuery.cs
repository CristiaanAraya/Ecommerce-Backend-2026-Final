using MediatR;

namespace ECommerce.Application.Features.Orders.Queries;

public record GetOrderByIdQuery(Guid OrderId, Guid RequestingUserId, bool IsAdmin) : IRequest<OrderDto>;
