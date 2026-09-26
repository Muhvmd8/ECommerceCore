using ECommerce.Domain.Entities.Common;
using System.Linq.Expressions;
namespace ECommerce.UseCases.Specifications.SpecificationBuilders.Include;
internal class IncludableSpecificationsBuilder<TEntity, TProperty>
    : SpecificationBuilder<TEntity>,
      IIncludableSpecificationsBuilder<TEntity, TProperty>
    where TEntity : BaseEntity
{
    private readonly LambdaExpression _parentExpression;

    internal IncludableSpecificationsBuilder(
        Specification<TEntity> specification,
        LambdaExpression parentExpression)
        : base(specification)
    {
        _parentExpression = parentExpression;
    }

    public IIncludableSpecificationsBuilder<TEntity, TNext> ThenInclude<TNext>(
        Expression<Func<TProperty, TNext>> includeExpression)
    {
        _specification.AddThenInclude(
            includeExpression,
            _parentExpression);

        return new IncludableSpecificationsBuilder<TEntity, TNext>(
            _specification,
            includeExpression);
    }
}

internal class IncludableCollectionSpecificationsBuilder<TEntity, TElement>
    : SpecificationBuilder<TEntity>, IIncludableCollectionSpecificationsBuilder<TEntity, TElement>
    where TEntity : BaseEntity
{
    private readonly LambdaExpression _parentExpression;
    internal IncludableCollectionSpecificationsBuilder(
        Specification<TEntity> specification,
        LambdaExpression parentExpression)
        : base(specification)
    {
        _parentExpression = parentExpression;
    }

    public IIncludableSpecificationsBuilder<TEntity, TNext> ThenInclude<TNext>(Expression<Func<TElement, TNext>> includeExpression)
    {
        _specification.AddThenInclude(includeExpression, _parentExpression);
        return new IncludableSpecificationsBuilder<TEntity, TNext>(_specification, includeExpression);
    }
}