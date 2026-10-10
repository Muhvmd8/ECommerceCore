using ECommerce.Domain.Constants;
using ECommerce.Domain.Entities.UserModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    public class UserAddressConfiguration
        : IEntityTypeConfiguration<UserAddress>
    {
        public virtual void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder.ToTable("UserAddresses");

            builder.Property(x => x.Country)
                .IsRequired()
                .HasMaxLength(UserAddressConstants.MaxCountryLength);

            builder.Property(x => x.City)
                .IsRequired()
                .HasMaxLength(UserAddressConstants.MaxCityLength);

            builder.Property(x => x.Street)
                .IsRequired()
                .HasMaxLength(UserAddressConstants.MaxStreetLength);

            builder.Property(x => x.RecipientFirstName)
                .IsRequired()
                .HasMaxLength(UserAddressConstants.MaxFirstNameLength);

            builder.Property(x => x.RecipientLastName)
                .IsRequired()
                .HasMaxLength(UserAddressConstants.MaxLastNameLength);

            builder.Property(x => x.Phone)
                .IsRequired()
                .HasMaxLength(UserAddressConstants.MaxPhoneLength);

            builder.HasIndex(x => x.ApplicationUserId, "IX_UserAddresses_ApplicationUserId");
        }
    }
}
