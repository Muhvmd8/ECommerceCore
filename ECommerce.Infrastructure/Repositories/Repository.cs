using ECommerce.Domain.Entities.Common;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using ECommerce.Domain.Specifications;
using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
namespace ECommerce.Infrastructure.Repositories;
public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : BaseEntity
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    #region Write Operations

    public async Task AddAsync(TEntity entity, CancellationToken ct = default) => await _dbSet.AddAsync(entity, ct);

    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken ct = default) => await _dbSet.AddRangeAsync(entities, ct);

    public void Update(TEntity entity) => _dbSet.Update(entity);

    public void UpdateRange(IEnumerable<TEntity> entities) => _dbSet.UpdateRange(entities);

    public void Delete(TEntity entity) => _dbSet.Remove(entity);

    public void DeleteRange(IEnumerable<TEntity> entities) => _dbSet.RemoveRange(entities);

    #endregion

    #region Read Operations

    public async Task<TEntity?> FirstOrDefaultAsync(
        ISpecifications<TEntity> specification,
        CancellationToken ct = default)
    {
        var query = SpecificationEvaluator.GetQuery(
            _dbSet.AsQueryable(),
            specification);

        return await query.FirstOrDefaultAsync(ct);
    }

    public async Task<TResult?> FirstOrDefaultAsync<TResult>(
        ISpecifications<TEntity, TResult> specification,
        CancellationToken ct = default)
    {
        var query = SpecificationEvaluator.GetQuery(
            _dbSet.AsQueryable(),
            specification);

        return await query.FirstOrDefaultAsync(ct);
    }

    public async Task<TEntity> SingleAsync(
        ISpecifications<TEntity> specification,
        CancellationToken ct = default)
    {
        var query = SpecificationEvaluator.GetQuery(
            _dbSet.AsQueryable(),
            specification);

        return await query.SingleAsync(ct);
    }

    public async Task<TResult> SingleAsync<TResult>(
        ISpecifications<TEntity, TResult> specification,
        CancellationToken ct = default)
    {
        var query = SpecificationEvaluator.GetQuery(
            _dbSet.AsQueryable(),
            specification);

        return await query.SingleAsync(ct);
    }

    public async Task<IReadOnlyList<TEntity>> ListAsync(
        ISpecifications<TEntity> specification,
        CancellationToken ct = default)
    {
        var query = SpecificationEvaluator.GetQuery(
            _dbSet.AsQueryable(),
            specification);

        return await query.ToListAsync(ct);
    }

    public async Task<IReadOnlyList<TResult>> ListAsync<TResult>(
        ISpecifications<TEntity, TResult> specification,
        CancellationToken ct = default)
    {
        var query = SpecificationEvaluator.GetQuery(
            _dbSet.AsQueryable(),
            specification);

        return await query.ToListAsync(ct);
    }

    public async Task<int> CountAsync(
        ISpecifications<TEntity> specification,
        CancellationToken ct = default)
    {
        var query = SpecificationEvaluator.GetQuery(
            _dbSet.AsQueryable(),
            specification);

        return await query.CountAsync(ct);
    }

    public async Task<bool> AnyAsync(
        ISpecifications<TEntity> specification,
        CancellationToken ct = default)
    {
        var query = SpecificationEvaluator.GetQuery(
            _dbSet.AsQueryable(),
            specification);

        return await query.AnyAsync(ct);
    }

    public async Task<PagedResult<TEntity>> PagedListAsync(
        ISpecifications<TEntity> specification,
        CancellationToken ct = default)
    {
        // Get total count before applying Skip / Take.
        var countQuery = SpecificationEvaluator.GetQuery(
            _dbSet.AsQueryable(),
            specification);

        var totalCount = await countQuery.CountAsync(ct);

        // Apply the complete specification including paging.
        var query = SpecificationEvaluator.GetQuery(
            _dbSet.AsQueryable(),
            specification);

        var items = await query.ToListAsync(ct);

        return new PagedResult<TEntity>(items, totalCount);
    }

    #endregion
}