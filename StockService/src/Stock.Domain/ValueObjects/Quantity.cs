using Stock.Domain.Exceptions;

namespace Stock.Domain.ValueObjects;

public class Quantity : ValueObject
{
    public int Value { get; }

    public Quantity(int value)
    {
        if (value < 0)
            throw new BusinessException("La cantidad no puede ser negativa.");
        Value = value;
    }

    public static Quantity operator +(Quantity a, Quantity b) => new(a.Value + b.Value);
    public static Quantity operator -(Quantity a, Quantity b) => new(a.Value - b.Value);

    public static implicit operator int(Quantity q) => q.Value;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
