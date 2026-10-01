using ECommerce.Domain.Entities.BasketModule;

namespace ECommerce.Domain.Repositories
{
    public interface IBasketRepository
    {
        Task<Basket?> GetAsync(Guid buyerId, CancellationToken ct = default);
        Task<Basket> GetOrCreateAsync(Guid buyerId, CancellationToken ct = default);
        Task SaveAsync(Basket basket, CancellationToken ct = default);
        Task DeleteAsync(Guid buyerId, CancellationToken ct = default);
    }
}
