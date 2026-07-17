using Stock.Domain.Exceptions;
using Stock.Domain.ValueObjects;

namespace Stock.Domain.Entities;

public class ProductStock
{
    public Guid ProductId { get; private set; }
    public ProductName ProductName { get; private set; } = null!;
    public Quantity TotalQuantity { get; private set; } = null!;
    public Quantity ReservedQuantity { get; private set; } = null!;
    public int AvailableQuantity => TotalQuantity.Value - ReservedQuantity.Value;

    private ProductStock() { }

    public ProductStock(Guid productId, string productName, int initialQuantity)
    {
        if (initialQuantity < 0)
            throw new BusinessException("La cantidad inicial no puede ser negativa.");

        ProductId = productId;
        ProductName = new ProductName(productName);
        TotalQuantity = new Quantity(initialQuantity);
        ReservedQuantity = new Quantity(0);
    }

    public void AddStock(int quantity)
    {
        if (quantity <= 0)
            throw new BusinessException("La cantidad a agregar debe ser mayor a cero.");

        TotalQuantity += new Quantity(quantity);
    }

    public bool CanReserve(int quantity)
    {
        return quantity > 0 && AvailableQuantity >= quantity;
    }

    public void Reserve(int quantity)
    {
        if (quantity <= 0)
            throw new BusinessException("La cantidad a reservar debe ser mayor a cero.");

        if (!CanReserve(quantity))
            throw new BusinessException(
                $"Stock insuficiente para el producto '{ProductName.Value}'. " +
                $"Disponible: {AvailableQuantity}, solicitado: {quantity}.");

        ReservedQuantity += new Quantity(quantity);
    }

    public void Release(int quantity)
    {
        if (quantity <= 0)
            throw new BusinessException("La cantidad a liberar debe ser mayor a cero.");

        if (quantity > ReservedQuantity.Value)
            throw new BusinessException(
                $"No se pueden liberar {quantity} unidades del producto '{ProductName.Value}'. " +
                $"Solo {ReservedQuantity.Value} están reservadas.");

        ReservedQuantity -= new Quantity(quantity);
    }

    public void UpdateProductName(string productName)
    {
        ProductName = new ProductName(productName);
    }
}
