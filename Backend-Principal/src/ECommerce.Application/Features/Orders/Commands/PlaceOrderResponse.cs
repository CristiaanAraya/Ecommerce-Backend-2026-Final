namespace ECommerce.Application.Features.Orders.Commands;

public record PlaceOrderResponse(
    Guid Id,
    Guid UserId,
    DateTime CreatedAt,
    string Status,
    decimal Total);
