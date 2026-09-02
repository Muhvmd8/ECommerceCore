using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.Infrastructure.Persistence.Seeding.Abstraction;
using ECommerce.Infrastructure.Persistence.Seeding.Models;
namespace ECommerce.Infrastructure.Persistence.Seeding.Implementation;
public class BrandSeeder(ApplicationDbContext context) : IDataSeeder
{
    public int Order => 1;

    public async Task SeedDataAsync(CancellationToken ct = default)
        => await JsonSeeder.SeedIfEmpty<ProductBrand, int, ProductBrandSeedModel>(
            context.Set<ProductBrand>(),
            "brands.json",
            model => ProductBrand.Create(model.Name),
            ct
        );
}
