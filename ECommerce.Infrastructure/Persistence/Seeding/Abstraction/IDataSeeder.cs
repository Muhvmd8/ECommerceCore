namespace ECommerce.Infrastructure.Persistence.Seeding.Abstraction;
public interface IDataSeeder
{
    int Order { get; }
    Task SeedDataAsync(CancellationToken ct = default);
}