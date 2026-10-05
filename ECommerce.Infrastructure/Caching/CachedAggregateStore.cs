using ECommerce.Infrastructure.Caching.Extensions;
using ECommerce.UseCases.Commons.Options;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;
namespace ECommerce.Infrastructure.Caching
{
    public sealed class CachedAggregateStore<T>(HybridCache cache,
        IOptionsMonitor<CacheEntryPolicy> options)
        : ICachedAggregateStore<T> where T : class
    {
        private readonly CacheEntryPolicy _options = options.Get(typeof(T).Name);
        public async Task<T?> GetAsync(string key, CancellationToken cancellationToken = default)
        {
            var envelope = await cache.TryGetAsync<CacheEnvelope<T>>(key, cancellationToken);
            return envelope?.Payload;
        }
        public async Task<T> GetOrCreateAsync(string key, Func<CancellationToken, Task<T>> factory, CancellationToken ct = default)
        {
            var envelope = await cache.GetOrCreateAsync
            (
                key,
                async cancel =>
                {
                    var value = await factory(cancel);
                    var utcNow = DateTimeOffset.UtcNow;

                    return new CacheEnvelope<T> { Payload = value, CreatedAtUtc = utcNow, LastAccessedAtUtc = utcNow };
                },
                _CreateEntryOptionsForNewEnvelope(),
                cancellationToken: ct
            );

            await _RefreshExpirationIfNeedAsync(key, envelope, ct);
            return envelope.Payload;
        }
        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
            => await cache.RemoveAsync(key, cancellationToken);
        public async Task SetAsync(string key, T value, CancellationToken cancellationToken = default)
        {
            var existing = await cache.TryGetAsync<CacheEnvelope<T>>(key, cancellationToken);
            if (existing is null) return;
            var envelope = new CacheEnvelope<T>
            {
                Payload = value,
                CreatedAtUtc = existing?.CreatedAtUtc ?? DateTimeOffset.UtcNow,
                LastAccessedAtUtc = DateTimeOffset.UtcNow
            };
            await _SetOrRemoveIfExpiredAsync(key, envelope, cancellationToken);
        }
        private async Task _RefreshExpirationIfNeedAsync(string key, CacheEnvelope<T> envelope, CancellationToken ct)
        {
            var utcNow = DateTimeOffset.UtcNow;
            var age = utcNow - envelope.LastAccessedAtUtc;

            if (age < TimeSpan.FromMinutes(_options.SlidingRefreshThresholdMinutes))
                return;

            var refreshed = new CacheEnvelope<T>
            {
                Payload = envelope.Payload,
                CreatedAtUtc = utcNow,
                LastAccessedAtUtc = utcNow,
            };

            await _SetOrRemoveIfExpiredAsync(key, refreshed, ct);
        }
        private async Task _SetOrRemoveIfExpiredAsync(string key, CacheEnvelope<T> refreshed, CancellationToken ct)
        {
            var expiration = _CalculateExpiration(
                refreshed.CreatedAtUtc,
                refreshed.LastAccessedAtUtc,
                DateTimeOffset.UtcNow);

            if (expiration is null)
            {
                await cache.RemoveAsync(key, ct);
                return;
            }

            await cache.SetAsync(key, refreshed, _CreateEntryOptions(expiration.Value), cancellationToken: ct);
        }
        private HybridCacheEntryOptions _CreateEntryOptionsForNewEnvelope()
        {
            var utcNow = DateTimeOffset.UtcNow;
            var expiration = _CalculateExpiration(utcNow, utcNow, utcNow) ??
                throw new InvalidOperationException();

            return _CreateEntryOptions(expiration);
        }
        private HybridCacheEntryOptions _CreateEntryOptions(TimeSpan expiration)
        {
            var localExpiration = TimeSpan.FromMinutes(_options.LocalCacheExpirationMinutes);

            if (localExpiration > expiration)
                localExpiration = expiration;

            return new HybridCacheEntryOptions
            {
                Expiration = expiration,
                LocalCacheExpiration = localExpiration
            };
        }
        private TimeSpan? _CalculateExpiration(
            DateTimeOffset createdAtUtc,
            DateTimeOffset lastAccessedAtUtc,
            DateTimeOffset utcNow)
        {
            var absoluteRemaining = createdAtUtc
                .AddDays(_options.AbsoluteExpirationDays)
                .Subtract(utcNow);

            var slidingRemaining = lastAccessedAtUtc
                .AddDays(_options.SlidingExpirationDays)
                .Subtract(utcNow);

            if (absoluteRemaining <= TimeSpan.Zero || slidingRemaining <= TimeSpan.Zero)
                return null;

            return absoluteRemaining <= slidingRemaining ? absoluteRemaining : slidingRemaining;
        }
    }
}