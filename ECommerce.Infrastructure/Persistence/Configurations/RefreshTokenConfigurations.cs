using ECommerce.Domain.Entities.Auth;
using ECommerce.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    public sealed class RefreshTokenConfiguration
        : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(
            EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.Property(x => x.Token)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.JwtId)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(x => x.Token)
                .IsUnique();

            builder.HasIndex(x => x.JwtId);

            builder.HasOne<ApplicationUser>()
                .WithMany(x => x.RefreshTokens)
                .HasForeignKey(x => x.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
