namespace ECommerce.Api.DTOs;

public record PlaceOrderRequest(List<OrderItemRequest> Items);
public record OrderItemRequest(Guid ProductId, int Quantity);

public record OrderResponse(
    Guid Id,
    Guid UserId,
    DateTime CreatedAt,
    string Status,
    decimal Total,
    List<OrderItemResponse> Items);

public record OrderItemResponse(
    Guid Id,
    Guid ProductId,
    decimal UnitPrice,
    int Quantity,
    decimal Subtotal);
