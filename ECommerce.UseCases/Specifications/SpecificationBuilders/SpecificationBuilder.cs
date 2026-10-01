using ECommerce.Domain.Entities.Common;
using ECommerce.Domain.Specifications;
using ECommerce.UseCases.Specifications.SpecificationBuilders.Include;
using ECommerce.UseCases.Specifications.SpecificationBuilders.Order;
using System.Linq.Expressions;
namespace ECommerce.UseCases.Specifications.SpecificationBuilders;
internal class SpecificationBuilder<TEntity>
    : ISpecificationsBuilder<TEntity>
    where TEntity : BaseEntity
{
    protected readonly Specification<TEntity> _specification;

    internal SpecificationBuilder(Specification<TEntity> specification)
    {
        _specification = specification;
    }
    public ISpecificationsBuilder<TEntity> AsNoTracking()
    {
        _specification.DisableTracking();
        return this;
    }

    public ISpecificationsBuilder<TEntity> AsTracking()
    {
        _specification.EnableTracking();
        return this;
    }

    public IIncludableSpecificationsBuilder<TEntity, TProperty> Include<TProperty>(
        Expression<Func<TEntity, TProperty>> includeExpression)
    {
        _specification.AddInclude(includeExpression);
        return new IncludableSpecificationsBuilder<TEntity, TProperty>(_specification, includeExpression);
    }

    public IIncludableCollectionSpecificationsBuilder<TEntity, TElement> IncludeCollection<TElement>(Expression<Func<TEntity, ICollection<TElement>>> includeExpression)
    {
        _specification.AddInclude(includeExpression);
        return new IncludableCollectionSpecificationsBuilder<TEntity, TElement>(_specification, includeExpression);
    }

    public IOrderSpecificationsBuilder<TEntity> OrderBy(Expression<Func<TEntity, object?>> orderByExpression)
    {
        _specification.AddOrderBy(orderByExpression!, OrderType.Ascending);
        return new OrderSpecificationsBuilder<TEntity>(_specification);
    }

    public IOrderSpecificationsBuilder<TEntity> OrderByDescending(Expression<Func<TEntity, object?>> orderByExpression)
    {
        _specification.AddOrderBy(orderByExpression!, OrderType.Descending);
        return new OrderSpecificationsBuilder<TEntity>(_specification);
    }

    public ISpecificationsBuilder<TEntity> Skip(int skip)
    {
        _specification.SetSkip(skip);
        return this;
    }

    public ISpecificationsBuilder<TEntity> Take(int take)
    {
        _specification.SetTake(take);
        return this;
    }

    public ISpecificationsBuilder<TEntity> Where(Expression<Func<TEntity, bool>> criteria)
    {
        _specification.AddCriteria(criteria);
        return this;
    }
}
internal class SpecificationBuilder<TEntity, TResult>
    : ISpecificationsBuilder<TEntity, TResult>
    where TEntity : BaseEntity
{
    private readonly Specification<TEntity, TResult> _specification;
    private readonly SpecificationBuilder<TEntity> _builder;

    internal SpecificationBuilder(
        Specification<TEntity, TResult> specification)
    {
        _specification = specification;
        _builder = new SpecificationBuilder<TEntity>(_specification);
    }

    // Where
    public ISpecificationsBuilder<TEntity, TResult> Where(
        Expression<Func<TEntity, bool>> criteria)
    {
        _builder.Where(criteria);
        return this;
    }

    // Tracking
    public ISpecificationsBuilder<TEntity, TResult> AsNoTracking()
    {
        _builder.AsNoTracking();
        return this;
    }
    public ISpecificationsBuilder<TEntity, TResult> AsTracking()
    {
        _builder.AsTracking();
        return this;
    }

    // Skip & Take
    public ISpecificationsBuilder<TEntity, TResult> Skip(int skip)
    {
        _builder.Skip(skip);
        return this;
    }
    public ISpecificationsBuilder<TEntity, TResult> Take(int take)
    {
        _builder.Take(take);
        return this;
    }

    // Ordering 
    public IOrderSpecificationsBuilder<TEntity> OrderBy(Expression<Func<TEntity, object?>> orderByExpression)
    {
        _specification.AddOrderBy(orderByExpression!, OrderType.Ascending);
        return new OrderSpecificationsBuilder<TEntity>(_specification);
    }
    public IOrderSpecificationsBuilder<TEntity> OrderByDescending(Expression<Func<TEntity, object?>> orderByExpression)
    {
        _specification.AddOrderBy(orderByExpression!, OrderType.Descending);
        return new OrderSpecificationsBuilder<TEntity>(_specification);
    }

    // Projection
    public ISpecificationsBuilder<TEntity, TResult> Select(
        Expression<Func<TEntity, TResult>> selector)
    {
        _specification.SetSelector(selector);
        return this;
    }
    public ISpecificationsBuilder<TEntity, TResult> SelectMany(Expression<Func<TEntity, IEnumerable<TResult>>> selector)
    {
        _specification.SetManySelector(selector);
        return this;
    }
}
