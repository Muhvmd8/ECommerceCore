using ECommerce.Domain.Entities.Common;
using ECommerce.Domain.Interfaces.Specifications;
using System.Linq.Expressions;
namespace ECommerce.UseCases.Specifications.SpecificationBuilders.Order;
internal class OrderSpecificationsBuilder<TEntity>
    : SpecificationBuilder<TEntity>, IOrderSpecificationsBuilder<TEntity>
    where TEntity : BaseEntity
{
    internal OrderSpecificationsBuilder(Specification<TEntity> specification)
        : base(specification) { }
    public IOrderSpecificationsBuilder<TEntity> ThenBy(Expression<Func<TEntity, object?>> orderByExpression)
    {
        _specification.AddOrderBy(orderByExpression!, OrderType.ThenAscending);
        return this;
    }
    public IOrderSpecificationsBuilder<TEntity> ThenByDescending(Expression<Func<TEntity, object?>> orderByExpression)
    {
        _specification.AddOrderBy(orderByExpression!, OrderType.ThenDescending);
        return this;
    }
}