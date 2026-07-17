namespace ECommerce.Domain.Exceptions;

public class InsufficientStockException : DomainException
{
    public InsufficientStockException(int requested, int available)
        : base($"Stock insuficiente: se solicitaron {requested} unidades pero solo hay {available} disponibles.") { }
}
