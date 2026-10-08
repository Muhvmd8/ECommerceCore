using ECommerce.Infrastructure.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    public sealed class EmailVerificationCodeConfigurations
        : IEntityTypeConfiguration<EmailVerificationCode>
    {
        public void Configure(
            EntityTypeBuilder<EmailVerificationCode> builder)
        {
            builder.ToTable("EmailVerificationCodes");

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(6);

            builder.HasIndex(x => new
            {
                x.ApplicationUserId,
                x.Code
            });

            builder.HasOne(e => e.ApplicationUser)
               .WithOne(u => u.EmailVerificationCode)
               .HasForeignKey<EmailVerificationCode>(e => e.ApplicationUserId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
