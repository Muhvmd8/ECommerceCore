using Microsoft.Extensions.Caching.Hybrid;

namespace ECommerce.Infrastructure.Caching.Extensions
{
    internal static class HybridCacheExtensions
    {
        private static readonly HybridCacheEntryOptions readOnlyOptions = new()
        {
            Flags = HybridCacheEntryFlags.DisableLocalCacheWrite |
            HybridCacheEntryFlags.DisableDistributedCacheWrite
        };

        public static async ValueTask<T?> TryGetAsync<T>(
            this HybridCache cache,
            string key,
            CancellationToken ct = default)
        {
            var found = false;

            var value = await cache.GetOrCreateAsync
            (
                key,
                _ =>
                {
                    found = false;
                    return ValueTask.FromResult<T?>(default);
                },
                readOnlyOptions,
                cancellationToken: ct
            );

            return found ? value : default;
        }
    }
}
