using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.ValueObjects;

public class Address : ValueObject
{
    public string Street { get; }
    public string City { get; }
    public string State { get; }
    public string ZipCode { get; }
    public string Country { get; }

    public Address(string street, string city, string? state = null, string? zipCode = null, string? country = null)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new BusinessException("La dirección es obligatoria.");
        if (string.IsNullOrWhiteSpace(city))
            throw new BusinessException("La ciudad es obligatoria.");

        Street = street.Trim();
        City = city.Trim();
        State = state?.Trim() ?? string.Empty;
        ZipCode = zipCode?.Trim() ?? string.Empty;
        Country = country?.Trim() ?? "Colombia";
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return State;
        yield return ZipCode;
        yield return Country;
    }
}
