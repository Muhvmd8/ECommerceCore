using ECommerce.Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    public sealed class BlockedIpConfiguration
        : IEntityTypeConfiguration<BlockedIp>
    {
        public void Configure(EntityTypeBuilder<BlockedIp> builder)
        {
            builder.ToTable("BlockedIps", "Security");

            builder.Property(b => b.IpAddress)
                .IsRequired()
                .HasMaxLength(45);
        }
    }
}
