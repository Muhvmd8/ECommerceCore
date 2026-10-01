using ECommerce.Domain.Entities.BasketModule;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Caching;

namespace ECommerce.Infrastructure.Repositories
{
    public class BasketRepository(ICachedAggregateStore<Basket> store)
        : IBasketRepository
    {
        public async Task DeleteAsync(Guid buyerId, CancellationToken ct = default)
            => await store.RemoveAsync(_BuildCacheKey(buyerId), ct);

        public async Task<Basket?> GetAsync(Guid buyerId, CancellationToken ct = default)
            => await store.GetAsync(_BuildCacheKey(buyerId), ct);

        public async Task<Basket> GetOrCreateAsync(Guid buyerId, CancellationToken ct = default)
            => await store.GetOrCreateAsync(
                _BuildCacheKey(buyerId),
                async cancel =>
                {
                    var createResult = Basket.CreateEmpty(buyerId);
                    await store.SetAsync(_BuildCacheKey(buyerId), createResult.Value, cancel);
                    return createResult.Value;
                },
                ct);

        public Task SaveAsync(Basket basket, CancellationToken ct = default)
            => store.SetAsync(_BuildCacheKey(basket.BuyerId), basket, ct);

        private string _BuildCacheKey(Guid buyerId) => $"basket:{buyerId}";
    }
}