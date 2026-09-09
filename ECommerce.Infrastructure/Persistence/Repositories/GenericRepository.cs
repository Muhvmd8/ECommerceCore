using ECommerce.Domain.Entities.Common;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace ECommerce.Infrastructure.Repositories;
public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
    where TEntity : BaseEntity<TKey>
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public async Task AddAsync(
        TEntity entity,
        CancellationToken ct = default)
        => await _dbSet.AddAsync(entity, ct);

    public async Task AddRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken ct = default)
        => await _dbSet.AddRangeAsync(entities, ct);

    public void Update(
        TEntity entity,
        CancellationToken ct = default)
        => _dbSet.Update(entity);

    public void UpdateRange(
        IEnumerable<TEntity> entities,
        CancellationToken ct = default)
        => _dbSet.UpdateRange(entities);

    public void Remove(
        TEntity entity,
        CancellationToken ct = default)
        => _dbSet.Remove(entity);

    public void RemoveRange(
        IEnumerable<TEntity> entities,
        CancellationToken ct = default)
        => _dbSet.RemoveRange(entities);

    public virtual async Task<TEntity?> GetByIdAsync(
        TKey key,
        CancellationToken ct = default)
        => await _dbSet.FindAsync([key], ct);

    public virtual async Task<TEntity?> GetFirstOrDefaultAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken ct = default,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;

        if (predicate != null)
            query = query.Where(predicate);

        if (includes is { Length: > 0 })
        {
            foreach (var include in includes)
                query = query.Include(include);
        }

        return await query.FirstOrDefaultAsync(ct);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool trackChanges = false,
        CancellationToken ct = default,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query =
            trackChanges ? _dbSet : _dbSet.AsNoTracking();

        if (predicate != null)
            query = query.Where(predicate);

        if (includes is { Length: > 0 })
        {
            foreach (var include in includes)
                query = query.Include(include);
        }

        return await query.ToListAsync(ct);
    }

    public virtual async Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken ct = default)
    {
        IQueryable<TEntity> query = _dbSet;

        if (predicate != null)
            query = query.Where(predicate);

        return await query.CountAsync(ct);
    }

    public virtual async Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken ct = default)
    {
        IQueryable<TEntity> query = _dbSet;

        if (predicate != null)
            query = query.Where(predicate);

        return await query.AnyAsync(ct);
    }

    public virtual IQueryable<TEntity> GetAllQueryable(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken ct = default,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;

        if (predicate != null)
            query = query.Where(predicate);

        if (includes is { Length: > 0 })
        {
            foreach (var include in includes)
                query = query.Include(include);
        }

        return query;
    }
}