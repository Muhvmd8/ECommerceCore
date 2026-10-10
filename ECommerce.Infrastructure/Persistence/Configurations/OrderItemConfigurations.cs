using ECommerce.Domain.Constants;
using ECommerce.Domain.Entities.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            builder.HasKey(x => x.Id);

            builder.OwnsOne(x => x.ItemOrdered, snapshot =>
            {
                snapshot.Property(p => p.ProductId)
                    .HasColumnName("ProductId")
                    .IsRequired();

                snapshot.Property(p => p.ProductName)
                    .HasColumnName("ProductName")
                    .IsRequired()
                    .HasMaxLength(ProductItemOrderedConstants.MaxProductNameLength);

                snapshot.Property(p => p.PictureUrl)
                    .HasColumnName("PictureUrl")
                    .IsRequired()
                    .HasMaxLength(ProductItemOrderedConstants.MaxPictureUrlLength);

                snapshot.Property(p => p.UnitPrice)
                    .HasColumnName("UnitPrice")
                    .HasPrecision(18, 2);

                snapshot.HasIndex(p => p.ProductId);
            });

            builder.Navigation(x => x.ItemOrdered).IsRequired();

            builder.HasIndex(x => x.OrderId);

            builder.Ignore(x => x.LineTotal);
        }
    }
}
