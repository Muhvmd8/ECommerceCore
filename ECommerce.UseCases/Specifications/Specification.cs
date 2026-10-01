using ECommerce.Domain.Entities.Common;
using ECommerce.Domain.Specifications;
using ECommerce.UseCases.Specifications.SpecificationBuilders;
using System.Linq.Expressions;

namespace ECommerce.UseCases.Specifications;

public abstract class Specification<TEntity>
    : ISpecifications<TEntity>
    where TEntity : BaseEntity
{
    private readonly List<Expression<Func<TEntity, bool>>> _criteria = [];
    private readonly List<Expression<Func<TEntity, object>>> _includes = [];
    private readonly List<ThenIncludesExpressionInfo<TEntity>> _thenIncludes = [];
    private readonly List<OrderExpressionInfo<TEntity>> _orderExpressions = [];

    // Store the builder instead of creating a new instance every time Query is accessed.
    private readonly ISpecificationsBuilder<TEntity> _builder;

    public Specification()
    {
        // Initialize the builder once and reuse it.
        _builder = new SpecificationBuilder<TEntity>(this);
    }

    // Builder
    protected internal ISpecificationsBuilder<TEntity> Query => _builder;

    public IReadOnlyList<Expression<Func<TEntity, bool>>> Criteria => _criteria;

    public IReadOnlyList<Expression<Func<TEntity, object>>> Includes => _includes;

    public IReadOnlyList<ThenIncludesExpressionInfo<TEntity>> ThenIncludes
        => _thenIncludes;

    public IReadOnlyList<OrderExpressionInfo<TEntity>> OrderByThenByExpressions
        => _orderExpressions;

    internal void AddCriteria(
        Expression<Func<TEntity, bool>> criteria)
        => _criteria.Add(criteria);

    internal Expression<Func<TEntity, object>> AddInclude<TProperty>(
        Expression<Func<TEntity, TProperty>> include)
    {
        Expression<Func<TEntity, object>> convertedInclude =
            Expression.Lambda<Func<TEntity, object>>(
                Expression.Convert(include.Body, typeof(object)),
                include.Parameters
            );

        _includes.Add(convertedInclude);

        return convertedInclude;
    }

    internal void AddThenInclude(
        LambdaExpression navigation,
        LambdaExpression parent)
    {
        _thenIncludes.Add(
            new ThenIncludesExpressionInfo<TEntity>
            {
                LambdaExpression = navigation,
                PreviousExpression = parent
            });
    }

    internal void AddOrderBy(
        Expression<Func<TEntity, object>> orderBy,
        OrderType orderType)
    {
        _orderExpressions.Add(
            new OrderExpressionInfo<TEntity>
            {
                KeySelector = orderBy!,
                OrderType = orderType
            });
    }

    internal void SetSkip(int skip)
        => Skip = skip;

    internal void SetTake(int take)
        => Take = take;

    internal void ApplyPagination(
        int? skip,
        int? take)
    {
        Skip = skip;
        Take = take;
    }

    internal void EnableTracking()
        => IsTrackingEnabled = true;

    internal void DisableTracking()
        => IsTrackingEnabled = false;

    public int? Skip { get; private set; }

    public int? Take { get; private set; }

    public bool IsPaginatedEnabled
        => Skip.HasValue || Take.HasValue;

    public bool IsTrackingEnabled { get; private set; }
}

// This class now owns the projection-specific state.
// The builder for this specification must be a projection builder
// so Query.Select(...) can preserve TResult.
public abstract class Specification<TEntity, TResult>
    : Specification<TEntity>,
      ISpecifications<TEntity, TResult>
    where TEntity : BaseEntity
{
    // Store the projection builder instead of using the base Query builder.
    protected new ISpecificationsBuilder<TEntity, TResult> Query => new SpecificationBuilder<TEntity, TResult>(this);

    public Expression<Func<TEntity, TResult>>? Selector
    {
        get;
        private set;
    }

    public Expression<Func<TEntity, IEnumerable<TResult>>>? ManySelector
    {
        get;
        private set;
    }

    internal void SetSelector(
        Expression<Func<TEntity, TResult>> selector)
        => Selector = selector;

    internal void SetManySelector(
        Expression<Func<TEntity, IEnumerable<TResult>>> manySelector)
        => ManySelector = manySelector;
}