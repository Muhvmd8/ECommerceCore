using ECommerce.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
namespace ECommerce.Infrastructure.Persistence.Interceptors;
internal sealed class AuditInterceptor : IAuditInterceptor
{
    public void Apply(DbContext dbContext)
    {
        var utcNow = DateTime.UtcNow;

        foreach (var entry in dbContext.ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
                entry.Entity.SetCreatedAt(utcNow);
            else if (entry.State == EntityState.Modified)
                entry.Entity.SetUpdatedAt(utcNow);
        }
    }
}
