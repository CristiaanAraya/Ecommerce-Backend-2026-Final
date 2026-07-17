using Stock.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Stock.Infrastructure.Persistence.Configurations;

public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
{
    public void Configure(EntityTypeBuilder<StockMovement> builder)
    {
        builder.ToTable("StockMovements");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).ValueGeneratedNever();

        builder.Property(m => m.ChangeType)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(m => m.Quantity).IsRequired();
        builder.Property(m => m.PreviousTotal).IsRequired();
        builder.Property(m => m.NewTotal).IsRequired();
        builder.Property(m => m.Reason).HasMaxLength(500);
        builder.Property(m => m.CreatedAt).IsRequired();

        builder.HasOne(m => m.Product)
            .WithMany()
            .HasForeignKey(m => m.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(m => m.ProductId);
        builder.HasIndex(m => m.CreatedAt);
    }
}
