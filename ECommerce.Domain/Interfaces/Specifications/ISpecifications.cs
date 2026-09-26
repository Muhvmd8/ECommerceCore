using ECommerce.Domain.Entities.Common;
using System.Linq.Expressions;

namespace ECommerce.Domain.Interfaces.Specifications;
public interface ISpecifications<TEntity>
    where TEntity : BaseEntity
{
    // Property signature for each dynamic part in the query.
    // 1. Where 
    public IReadOnlyList<Expression<Func<TEntity, bool>>> Criteria { get; }
    public IReadOnlyList<Expression<Func<TEntity, object>>> Includes { get; }
    public IReadOnlyList<ThenIncludesExpressionInfo<TEntity>> ThenIncludes { get; }
    public IReadOnlyList<OrderExpressionInfo<TEntity>> OrderByThenByExpressions { get; }
    /// public Expression<Func<TEntity, object>> OrderBy { get; }
    /// public Expression<Func<TEntity, object>> OrderByDescending { get; }
    public int? Take { get; }
    public int? Skip { get; }
    public bool IsPaginatedEnabled { get; }
    public bool IsTrackingEnabled { get; }
}

public interface ISpecifications<TEntity, TResult>
    : ISpecifications<TEntity>
    where TEntity : BaseEntity
{
    public Expression<Func<TEntity, TResult>>? Selector { get; }
    public Expression<Func<TEntity, IEnumerable<TResult>>>? ManySelector { get; }

}