using Stock.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Stock.Infrastructure.Persistence.Configurations;

public class QuantityConverter : ValueConverter<Quantity, int>
{
    public QuantityConverter()
        : base(q => q.Value, i => new Quantity(i)) { }
}

public class ProductNameConverter : ValueConverter<ProductName, string>
{
    public ProductNameConverter()
        : base(n => n.Value, s => new ProductName(s)) { }
}
