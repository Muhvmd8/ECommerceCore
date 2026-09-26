using ECommerce.Domain.Common;
using ECommerce.Domain.Entities.Common;
using ECommerce.Domain.Interfaces.Specifications;
namespace ECommerce.Domain.Interfaces;
public interface IReadRepository<TEntity>
    where TEntity : BaseEntity
{
    Task<TEntity?> FirstOrDefaultAsync(
        ISpecifications<TEntity> specification,
        CancellationToken ct = default);

    Task<TResult?> FirstOrDefaultAsync<TResult>(
        ISpecifications<TEntity, TResult> specification,
        CancellationToken ct = default);

    Task<TEntity> SingleAsync(
        ISpecifications<TEntity> specification,
        CancellationToken ct = default);

    Task<TResult> SingleAsync<TResult>(
        ISpecifications<TEntity, TResult> specification,
        CancellationToken ct = default);

    Task<IReadOnlyList<TEntity>> ListAsync(
        ISpecifications<TEntity> specification,
        CancellationToken ct = default);

    Task<IReadOnlyList<TResult>> ListAsync<TResult>(
        ISpecifications<TEntity, TResult> specification,
        CancellationToken ct = default);

    Task<int> CountAsync(
        ISpecifications<TEntity> specification,
        CancellationToken ct = default);

    Task<bool> AnyAsync(
        ISpecifications<TEntity> specification,
        CancellationToken ct = default);

    Task<PagedResult<TEntity>> PagedListAsync(
        ISpecifications<TEntity> specification,
        CancellationToken ct = default);
}
