using ECommerce.Domain.Entities.Common;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.Infrastructure.Persistence.Interceptors;
using System.Collections.Concurrent;
namespace ECommerce.Infrastructure.Repositories;
public class UnitOfWork(ApplicationDbContext _context,
    ISoftDeleteInterceptor softDeleteInterceptor,
    IAuditInterceptor auditInterceptor)
    : IUnitOfWork
{
    private readonly ConcurrentDictionary<string/*typeName*/, object/*Repo*/> _repositories = [];
    public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
        where TEntity : BaseEntity<TKey>
    {
        var typeName = typeof(TEntity).Name;
        // Get repo from the container
        if (_repositories.ContainsKey(typeName))
            return (IGenericRepository<TEntity, TKey>)_repositories[typeName];

        var repo = new GenericRepository<TEntity, TKey>(_context);
        _repositories[typeName] = repo;
        return repo;
    }
    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        auditInterceptor.Apply(_context);
        softDeleteInterceptor.Apply(_context);
        return await _context.SaveChangesAsync(ct);
    }
    public async Task BeginTransactionAsync(CancellationToken ct = default) => await _context.Database.BeginTransactionAsync(ct);
    public async Task CommitTransactionAsync(CancellationToken ct = default) => await _context.Database.CommitTransactionAsync(ct);
    public async Task RollbackTransactionAsync(CancellationToken ct = default) => await _context.Database.RollbackTransactionAsync(ct);
}
