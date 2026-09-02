using ECommerce.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
namespace ECommerce.Infrastructure.Persistence.Seeding;
public static class JsonSeeder
{
    public static readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
    };

    public static async Task SeedIfEmpty<TEntity, TKey, TModel>(
        DbSet<TEntity> dbSet,
        string fileName,
        Func<TModel, TEntity> map, // projection
        CancellationToken ct)
        where TEntity : BaseEntity<TKey>
        where TModel : class
    {

        if (await dbSet.AnyAsync(ct)) return;

        var filePath = Path.Combine(AppContext.BaseDirectory, "Persistence", "Seeding", "Data", fileName);

        if (!File.Exists(filePath)) return;

        await using var stream = File.OpenRead(filePath);

        var models = await JsonSerializer.DeserializeAsync<List<TModel>>(stream, _options, ct);

        if (models is null || !models.Any()) return;

        var entities = models.Select(map).ToList();

        await dbSet.AddRangeAsync(entities, ct);
    }
}