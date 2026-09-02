using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.Infrastructure.Persistence.Seeding.Abstraction;
namespace ECommerce.Infrastructure.Persistence.Seeding;
public class DatabaseSeeder(ApplicationDbContext context,
    IEnumerable<IDataSeeder> seeders)
{
    public async Task SeedAsync(CancellationToken ct = default)
    {
        foreach (var seeder in seeders.OrderBy(s => s.Order))
        {
            await seeder.SeedDataAsync(ct);
            await context.SaveChangesAsync(ct);
        }
    }
}