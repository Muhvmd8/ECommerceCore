using ECommerce.Domain.Constants;
using ECommerce.Domain.Entities.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    public sealed class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            BaseEntityConfiguration.Configure(builder);

            builder.ToTable("DeliveryMethods");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(DeliveryMethodConstants.MaxNameLength);

            builder.Property(x => x.Description)
                .HasMaxLength(DeliveryMethodConstants.MaxDescriptionLength);

            builder.Property(x => x.Price)
                .HasPrecision(18, 2);

            builder.Property(x => x.EstimatedDeliveryTime)
                .IsRequired()
                .HasMaxLength(DeliveryMethodConstants.MaxDeliveryTimeLength);

            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.DisplayOrder);
        }
    }
}
