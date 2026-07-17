using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public static readonly Guid ElectronicaId = new Guid("a1b2c3d4-0000-0000-0000-000000000001");
    public static readonly Guid RopaId        = new Guid("a1b2c3d4-0000-0000-0000-000000000002");
    public static readonly Guid HogarId       = new Guid("a1b2c3d4-0000-0000-0000-000000000003");

    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);

        builder.HasData(
            new Category(ElectronicaId, "Electrónica"),
            new Category(RopaId,        "Ropa"),
            new Category(HogarId,       "Hogar")
        );
    }
}
