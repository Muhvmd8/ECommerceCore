using ECommerce.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
namespace ECommerce.Infrastructure.Persistence.Seeding;
public static class JsonSeeder
{
    public static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public static readonly string DataFolder =
        Path.Combine(AppContext.BaseDirectory, "Persistence", "Seeding", "Data");

    // Sync mapper (unchanged behavior)
    public static async Task SeedIfEmpty<TEntity, TKey, TModel>(
        DbSet<TEntity> dbSet,
        string fileName,
        Func<TModel, TEntity> map,
        CancellationToken ct)
        where TEntity : BaseEntity<TKey>
        where TModel : class
    {
        if (await dbSet.AnyAsync(ct)) return;

        var models = await ReadModelsAsync<TModel>(fileName, ct);
        if (models.Count == 0) return;

        await dbSet.AddRangeAsync(models.Select(map).ToList(), ct);
    }

    // Async mapper (for seeders that need I/O, e.g. image upload)
    public static async Task SeedIfEmptyAsync<TEntity, TKey, TModel>(
        DbSet<TEntity> dbSet,
        string fileName,
        Func<TModel, CancellationToken, Task<TEntity?>> mapAsync,
        CancellationToken ct)
        where TEntity : BaseEntity<TKey>
        where TModel : class
    {
        if (await dbSet.AnyAsync(ct)) return;

        var models = await ReadModelsAsync<TModel>(fileName, ct);
        if (models.Count == 0) return;

        var entities = new List<TEntity>();
        foreach (var model in models)
        {
            var entity = await mapAsync(model, ct);
            if (entity is not null)
                entities.Add(entity);
        }

        await dbSet.AddRangeAsync(entities, ct);
    }

    private static async Task<List<TModel>> ReadModelsAsync<TModel>(string fileName, CancellationToken ct)
    {
        var filePath = Path.Combine(DataFolder, fileName);
        if (!File.Exists(filePath)) return [];

        await using var stream = File.OpenRead(filePath);
        return await JsonSerializer.DeserializeAsync<List<TModel>>(stream, _options, ct) ?? [];
    }
}