using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.ValueObjects;

public class Money : ValueObject
{
    public decimal Amount { get; }

    public Money(decimal amount)
    {
        if (amount < 0)
            throw new BusinessException("El monto no puede ser negativo.");
        Amount = amount;
    }

    public static Money operator +(Money a, Money b) => new(a.Amount + b.Amount);
    public static Money operator *(Money money, int multiplier) => new(money.Amount * multiplier);

    public static implicit operator decimal(Money money) => money.Amount;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
    }
}
