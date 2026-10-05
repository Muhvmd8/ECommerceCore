using ECommerce.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    public abstract class BaseEntityConfiguration
    {
        public static void Configure<TEntity>(EntityTypeBuilder<TEntity> builder)
            where TEntity : BaseEntity
        {

            builder.Property(x => x.IsDeleted)
                .HasDefaultValue(false);

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
