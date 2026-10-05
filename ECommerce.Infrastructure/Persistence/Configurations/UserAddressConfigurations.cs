using ECommerce.Domain.Consts;
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
                .HasMaxLength(UserAddressConsts.MaxCountryLength);

            builder.Property(x => x.City)
                .IsRequired()
                .HasMaxLength(UserAddressConsts.MaxCityLength);

            builder.Property(x => x.Street)
                .IsRequired()
                .HasMaxLength(UserAddressConsts.MaxStreetLength);

            builder.Property(x => x.RecipientFirstName)
                .IsRequired()
                .HasMaxLength(UserAddressConsts.MaxFirstNameLength);

            builder.Property(x => x.RecipientLastName)
                .IsRequired()
                .HasMaxLength(UserAddressConsts.MaxLastNameLength);

            builder.Property(x => x.Phone)
                .IsRequired()
                .HasMaxLength(UserAddressConsts.MaxPhoneLength);

            builder.HasIndex(x => x.ApplicationUserId, "IX_UserAddresses_ApplicationUserId");
        }
    }
}
