using ECommerce.Domain.Exceptions;
using ECommerce.Domain.ValueObjects;

namespace ECommerce.Domain.Entities;

public enum OrderStatus { Pending, Confirmed, Shipped, Delivered, Cancelled }

public class Order
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public OrderStatus Status { get; private set; }
    public Money Total { get; private set; } = null!;

    public Address? ShippingAddress { get; private set; }

    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    private Order() { }

    public Order(Guid userId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        Status = OrderStatus.Pending;
        Total = new Money(0);
    }

    public void AddItem(Product product, int quantity)
    {
        product.DiscountStock(quantity);
        var item = new OrderItem(Id, product.Id, product.Price, quantity);
        _items.Add(item);
        Total += item.Subtotal;
    }

    public void SetShippingAddress(string street, string city, string? state = null, string? zipCode = null, string? country = null)
    {
        ShippingAddress = new Address(street, city, state, zipCode, country);
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new BusinessException("Solo se pueden confirmar órdenes en estado Pendiente.");
        Status = OrderStatus.Confirmed;
    }

    public void Cancel()
    {
        if (Status is OrderStatus.Shipped or OrderStatus.Delivered)
            throw new BusinessException("No se puede cancelar una orden que ya fue enviada o entregada.");
        Status = OrderStatus.Cancelled;
    }
}
