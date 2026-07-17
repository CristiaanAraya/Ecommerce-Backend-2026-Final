using MediatR;

namespace ECommerce.Application.Features.Orders.Commands;

public record OrderLine(Guid ProductId, int Quantity);

public record PlaceOrderCommand(Guid UserId, List<OrderLine> Lines) : IRequest<PlaceOrderResponse>;
