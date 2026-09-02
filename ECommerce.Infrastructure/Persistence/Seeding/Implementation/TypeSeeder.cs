using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.Infrastructure.Persistence.Seeding.Abstraction;
using ECommerce.Infrastructure.Persistence.Seeding.Models;
namespace ECommerce.Infrastructure.Persistence.Seeding.Implementation;
public class TypeSeeder(ApplicationDbContext context) : IDataSeeder
{
    public int Order => 2;

    public async Task SeedDataAsync(CancellationToken ct = default)
        => await JsonSeeder.SeedIfEmpty<ProductType, int, ProductTypeSeedModel>(
            context.Set<ProductType>(),
            "types.json",
            model => ProductType.Create(model.Name),
            ct
        );
}