using ECommerce.Domain.Entities.Common;
using ECommerce.UseCases.Specifications.SpecificationBuilders.Include;
using ECommerce.UseCases.Specifications.SpecificationBuilders.Order;
using System.Linq.Expressions;
namespace ECommerce.UseCases.Specifications.SpecificationBuilders;
public interface ISpecificationsBuilder<TEntity>
    where TEntity : BaseEntity
{
    // Where
    ISpecificationsBuilder<TEntity> Where(Expression<Func<TEntity, bool>> criteria);

    // Include
    IIncludableSpecificationsBuilder<TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> includeExpression);
    IIncludableCollectionSpecificationsBuilder<TEntity, TElement> IncludeCollection<TElement>(
        Expression<Func<TEntity, ICollection<TElement>>> includeExpression);

    // Ordering
    IOrderSpecificationsBuilder<TEntity> OrderBy(Expression<Func<TEntity, object?>> orderByExpression);
    IOrderSpecificationsBuilder<TEntity> OrderByDescending(Expression<Func<TEntity, object?>> orderByExpression);

    // Skip & Take
    ISpecificationsBuilder<TEntity> Skip(int skip);
    ISpecificationsBuilder<TEntity> Take(int take);

    // Tracking
    ISpecificationsBuilder<TEntity> AsNoTracking();
    ISpecificationsBuilder<TEntity> AsTracking();
}

public interface ISpecificationsBuilder<TEntity, TResult>
    where TEntity : BaseEntity
{
    // Where
    ISpecificationsBuilder<TEntity, TResult> Where(
        Expression<Func<TEntity, bool>> criteria);

    // Ordering
    IOrderSpecificationsBuilder<TEntity> OrderBy(Expression<Func<TEntity, object?>> orderByExpression);
    IOrderSpecificationsBuilder<TEntity> OrderByDescending(Expression<Func<TEntity, object?>> orderByExpression);

    // Skip & Take
    ISpecificationsBuilder<TEntity, TResult> Skip(int skip);
    ISpecificationsBuilder<TEntity, TResult> Take(int take);

    // Tracking
    ISpecificationsBuilder<TEntity, TResult> AsNoTracking();
    ISpecificationsBuilder<TEntity, TResult> AsTracking();

    // Selection
    ISpecificationsBuilder<TEntity, TResult> Select(Expression<Func<TEntity, TResult>> selector);
    ISpecificationsBuilder<TEntity, TResult> SelectMany(Expression<Func<TEntity, IEnumerable<TResult>>> selector);
}