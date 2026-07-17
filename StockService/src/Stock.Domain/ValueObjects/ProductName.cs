using Stock.Domain.Exceptions;

namespace Stock.Domain.ValueObjects;

public class ProductName : ValueObject
{
    public string Value { get; }

    public ProductName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new BusinessException("El nombre del producto es obligatorio.");
        if (value.Trim().Length > 200)
            throw new BusinessException("El nombre del producto no puede superar los 200 caracteres.");
        Value = value.Trim();
    }

    public static implicit operator string(ProductName name) => name.Value;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
