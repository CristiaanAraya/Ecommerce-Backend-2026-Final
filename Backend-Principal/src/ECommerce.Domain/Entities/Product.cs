using ECommerce.Domain.Exceptions;
using ECommerce.Domain.ValueObjects;

namespace ECommerce.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Money Price { get; private set; } = null!;
    public int Stock { get; private set; }
    public Guid CategoryId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsActive { get; private set; }

    public Category? Category { get; private set; }

    private Product() { }

    public static Product New(string name, string description, decimal price, int stock, Guid categoryId, Guid? id = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessException("El nombre del producto es obligatorio.");
        if (price <= 0)
            throw new BusinessException("El precio debe ser mayor a cero.");
        if (stock < 0)
            throw new BusinessException("El stock no puede ser negativo.");
        if (categoryId == Guid.Empty)
            throw new BusinessException("La categoría es obligatoria.");

        return new Product
        {
            Id = id ?? Guid.NewGuid(),
            Name = name.Trim(),
            Description = description?.Trim() ?? string.Empty,
            Price = new Money(price),
            Stock = stock,
            CategoryId = categoryId,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };
    }

    public void Edit(string name, string description, decimal price, int stock, Guid categoryId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessException("El nombre del producto es obligatorio.");
        if (price <= 0)
            throw new BusinessException("El precio debe ser mayor a cero.");
        if (stock < 0)
            throw new BusinessException("El stock no puede ser negativo.");
        if (categoryId == Guid.Empty)
            throw new BusinessException("La categoría es obligatoria.");

        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        Price = new Money(price);
        Stock = stock;
        CategoryId = categoryId;
    }

    public void ReplenishStock(int quantity)
    {
        if (quantity <= 0)
            throw new BusinessException("La cantidad a agregar debe ser mayor a cero.");
        Stock += quantity;
    }

    public void DiscountStock(int quantity)
    {
        if (quantity <= 0)
            throw new BusinessException("La cantidad debe ser mayor a cero.");
        if (quantity > Stock)
            throw new InsufficientStockException(quantity, Stock);
        Stock -= quantity;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
