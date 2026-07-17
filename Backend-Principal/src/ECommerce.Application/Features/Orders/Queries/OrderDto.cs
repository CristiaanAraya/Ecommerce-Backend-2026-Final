namespace ECommerce.Application.Features.Orders.Queries;

public record OrderItemDto(Guid Id, Guid ProductId, decimal UnitPrice, int Quantity, decimal Subtotal);

public record OrderDto(
    Guid Id,
    Guid UserId,
    DateTime CreatedAt,
    string Status,
    decimal Total,
    List<OrderItemDto> Items);
