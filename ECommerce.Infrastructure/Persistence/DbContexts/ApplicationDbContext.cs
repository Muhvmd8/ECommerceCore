using ECommerce.Domain.Entities.OrderModule;
using ECommerce.Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence.DbContexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<LoginAudit> LoginAudits { get; set; } = default!;
    public DbSet<BlockedIp> BlockedIps { get; set; } = default!;
    public DbSet<DeliveryMethod> DeliveryMethods { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly,
            type => type.Namespace == "ECommerce.Infrastructure.Persistence.Configurations");
        base.OnModelCreating(modelBuilder);
    }
}