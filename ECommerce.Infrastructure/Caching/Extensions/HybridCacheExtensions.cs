using Microsoft.Extensions.Caching.Hybrid;

namespace ECommerce.Infrastructure.Caching.Extensions;

internal static class HybridCacheExtensions
{
    private static readonly HybridCacheEntryOptions ReadOnlyOptions = new()
    {
        Flags =
            HybridCacheEntryFlags.DisableLocalCacheWrite |
            HybridCacheEntryFlags.DisableDistributedCacheWrite
    };

    public static async ValueTask<T?> TryGetAsync<T>(
        this HybridCache cache,
        string key,
        CancellationToken ct = default)
        where T : class
    {
        var factoryCalled = false;

        var value = await cache.GetOrCreateAsync<T?>(
            key,
            _ =>
            {
                factoryCalled = true;
                return ValueTask.FromResult<T?>(null);
            },
            ReadOnlyOptions,
            cancellationToken: ct);

        return factoryCalled ? null : value;
    }
}