using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.ValueObjects;

public class Email : ValueObject
{
    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new BusinessException("El email es obligatorio.");
        if (!value.Contains('@'))
            throw new BusinessException("El formato del email no es válido.");
        Value = value.Trim().ToLower();
    }

    public static implicit operator string(Email email) => email.Value;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
