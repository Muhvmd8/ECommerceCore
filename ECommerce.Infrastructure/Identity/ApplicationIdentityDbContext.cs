using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Identity
{
    public class ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options)
        : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
    {
        override protected void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationIdentityDbContext).Assembly,
                type => type.Namespace == "ECommerce.Infrastructure.Identity");
        }
    }
}