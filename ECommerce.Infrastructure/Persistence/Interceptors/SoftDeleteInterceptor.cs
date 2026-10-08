using ECommerce.Domain.Entities.Common;
using ECommerce.Infrastructure.Email;
using Microsoft.EntityFrameworkCore;
namespace ECommerce.Infrastructure.Persistence.Interceptors;
internal class SoftDeleteInterceptor : ISoftDeleteInterceptor
{
    public void Apply(DbContext dbContext)
    {
        foreach (var entry in dbContext.ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State != EntityState.Deleted)
                continue;
            if (entry.Entity is EmailVerificationCode)
                continue;

            entry.State = EntityState.Modified;
            entry.Entity.MarkAsDeleted();
        }
    }
}
