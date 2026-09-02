using ECommerce.Domain.Entities.ProductModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;
internal class ProductConfigurations : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(p => p.Price)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.HasOne(p => p.ProductBrand)
            .WithMany(b => b.Products)
            .HasForeignKey(p => p.ProductBrandId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.ProductType)
            .WithMany(t => t.Products)
            .HasForeignKey(p => p.ProductTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(p => p.Name)
            .IsUnique();

        builder.HasIndex(p => p.Price);
    }
}
