using ECommerce.Domain.Entities.Common;
using System.Linq.Expressions;
namespace ECommerce.Domain.Interfaces;
public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    Task AddAsync(TEntity entity, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken ct = default);
    void Update(TEntity entity, CancellationToken ct = default);
    void UpdateRange(IEnumerable<TEntity> entities, CancellationToken ct = default);
    void Remove(TEntity entity, CancellationToken ct = default);
    void RemoveRange(IEnumerable<TEntity> entities, CancellationToken ct = default);
    Task<TEntity?> GetByIdAsync(TKey key, CancellationToken ct = default);
    Task<TEntity?> GetFirstOrDefaultAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken ct = default,
        params Expression<Func<TEntity, object>>[] includes);

    Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool trackChanges = false,
        CancellationToken ct = default,
        params Expression<Func<TEntity, object>>[] includes);

    Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken ct = default);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken ct = default);
    IQueryable<TEntity> GetAllQueryable(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken ct = default,
        params Expression<Func<TEntity, object>>[] includes);
}
