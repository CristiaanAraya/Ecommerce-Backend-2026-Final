using Stock.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Stock.Infrastructure.Persistence.Configurations;

public class ProductStockConfiguration : IEntityTypeConfiguration<ProductStock>
{
    public void Configure(EntityTypeBuilder<ProductStock> builder)
    {
        builder.ToTable("ProductStocks");
        builder.HasKey(p => p.ProductId);
        builder.Property(p => p.ProductId).ValueGeneratedNever();

        builder.Property(p => p.ProductName)
            .HasConversion<ProductNameConverter>()
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.TotalQuantity)
            .HasConversion<QuantityConverter>()
            .IsRequired();

        builder.Property(p => p.ReservedQuantity)
            .HasConversion<QuantityConverter>()
            .IsRequired();

        builder.Ignore(p => p.AvailableQuantity);
    }
}
