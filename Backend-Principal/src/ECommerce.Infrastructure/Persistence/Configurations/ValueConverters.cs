using ECommerce.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public class MoneyConverter : ValueConverter<Money, decimal>
{
    public MoneyConverter()
        : base(m => m.Amount, d => new Money(d)) { }
}

public class EmailConverter : ValueConverter<Email, string>
{
    public EmailConverter()
        : base(e => e.Value, s => new Email(s)) { }
}
