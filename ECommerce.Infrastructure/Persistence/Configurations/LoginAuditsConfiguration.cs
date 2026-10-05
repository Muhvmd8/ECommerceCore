using ECommerce.Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    public sealed class LoginAuditConfiguration
        : IEntityTypeConfiguration<LoginAudit>
    {
        public void Configure(EntityTypeBuilder<LoginAudit> builder)
        {
            builder.ToTable("LoginAudits", "Security");

            builder.Property(b => b.Username)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
