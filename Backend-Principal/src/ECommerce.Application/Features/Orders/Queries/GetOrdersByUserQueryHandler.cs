using ECommerce.Domain.Contracts.Persistence;
using MediatR;

namespace ECommerce.Application.Features.Orders.Queries;

public class GetOrdersByUserQueryHandler(IOrderRepository orderRepository)
    : IRequestHandler<GetOrdersByUserQuery, IEnumerable<OrderDto>>
{
    public async Task<IEnumerable<OrderDto>> Handle(GetOrdersByUserQuery request, CancellationToken ct)
    {
        var orders = await orderRepository.GetByUserIdAsync(request.UserId, ct);
        return orders.Select(ToDto);
    }

    private static OrderDto ToDto(Domain.Entities.Order o) =>
        new(o.Id, o.UserId, o.CreatedAt, o.Status.ToString(), o.Total,
            o.Items.Select(i => new OrderItemDto(i.Id, i.ProductId, i.UnitPrice, i.Quantity, i.Subtotal)).ToList());
}
