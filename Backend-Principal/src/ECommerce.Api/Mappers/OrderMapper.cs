using ECommerce.Api.DTOs;
using ECommerce.Domain.Entities;

namespace ECommerce.Api.Mappers;

public static class OrderMapper
{
    public static OrderResponse ToResponse(Order order) =>
        new(order.Id,
            order.UserId,
            order.CreatedAt,
            order.Status.ToString(),
            order.Total,
            order.Items.Select(i => new OrderItemResponse(
                i.Id, i.ProductId, i.UnitPrice, i.Quantity, i.Subtotal)).ToList());
}
