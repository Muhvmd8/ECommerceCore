using ECommerce.Domain.Entities.Common;
using System.Linq.Expressions;
namespace ECommerce.UseCases.Specifications.SpecificationBuilders.Include;
public interface IIncludableSpecificationsBuilder<TEntity, TProperty>
    : ISpecificationsBuilder<TEntity>
    where TEntity : BaseEntity
{
    IIncludableSpecificationsBuilder<TEntity, TNext> ThenInclude<TNext>(
        Expression<Func<TProperty, TNext>> includeExpression);
}
public interface IIncludableCollectionSpecificationsBuilder<TEntity, TElement>
    : ISpecificationsBuilder<TEntity>
    where TEntity : BaseEntity
{
    IIncludableSpecificationsBuilder<TEntity, TNext> ThenInclude<TNext>(
        Expression<Func<TElement, TNext>> includeExpression);
}