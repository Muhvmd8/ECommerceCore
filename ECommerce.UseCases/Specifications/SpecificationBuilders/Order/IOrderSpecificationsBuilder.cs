using ECommerce.Domain.Entities.Common;
using System.Linq.Expressions;
namespace ECommerce.UseCases.Specifications.SpecificationBuilders.Order;

public interface IOrderSpecificationsBuilder<TEntity> : ISpecificationsBuilder<TEntity>
    where TEntity : BaseEntity
{
    IOrderSpecificationsBuilder<TEntity> ThenBy(Expression<Func<TEntity, object?>> orderByExpression);
    IOrderSpecificationsBuilder<TEntity> ThenByDescending(Expression<Func<TEntity, object?>> orderByExpression);
}
