using ECommerce.Domain.Entities.Common;
using ECommerce.Domain.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace ECommerce.Infrastructure.Specifications;
internal static class SpecificationEvaluator
{
    public static IQueryable<TEntity> GetQuery<TEntity>(
        IQueryable<TEntity> inputQuery,
        ISpecifications<TEntity> specification)
        where TEntity : BaseEntity
    {
        var query = inputQuery;

        query = ApplyCriteria(query, specification);

        query = ApplyIncludes(query, specification);

        query = ApplyOrdering(query, specification);

        query = ApplyTracking(query, specification);

        query = ApplyPagination(query, specification);

        return query;
    }

    public static IQueryable<TResult> GetQuery<TEntity, TResult>(
        IQueryable<TEntity> inputQuery,
        ISpecifications<TEntity, TResult> specification)
        where TEntity : BaseEntity
    {
        var query = GetQuery(
            inputQuery,
            (ISpecifications<TEntity>)specification);

        if (specification.Selector is not null)
            return query.Select(specification.Selector);

        if (specification.ManySelector is not null)
            return query.SelectMany(specification.ManySelector);

        throw new InvalidOperationException(
            $"Specification '{specification.GetType().Name}' " +
            "does not contain a projection.");
    }

    // ============================================================
    // WHERE
    // ============================================================

    private static IQueryable<TEntity> ApplyCriteria<TEntity>(
        IQueryable<TEntity> query,
        ISpecifications<TEntity> specification)
        where TEntity : BaseEntity
    {
        foreach (var criteria in specification.Criteria)
            query = query.Where(criteria);

        return query;
    }

    // ============================================================
    // INCLUDE / THEN INCLUDE
    // ============================================================

    private static IQueryable<TEntity> ApplyIncludes<TEntity>(
        IQueryable<TEntity> query,
        ISpecifications<TEntity> specification)
        where TEntity : BaseEntity
    {
        /*
         * We keep track of every navigation expression and its
         * corresponding property/member name.
         *
         * Example:
         *
         * Expression:
         *     customer => customer.Address
         *
         * Member name:
         *     Address
         *
         * Dictionary:
         *
         * Expression -> "Address"
         */

        var expressionPropertyNames =
            new Dictionary<LambdaExpression, string>();

        foreach (var include in specification.Includes)
        {
            var propertyName = GetMemberName(include);

            expressionPropertyNames[include] = propertyName;

            query = query.Include(propertyName);
        }

        /*
         * ThenIncludes are stored separately.
         *
         * Example:
         *
         * Include:
         *     customer => customer.Address
         *
         * ThenInclude:
         *     address => address.City
         *
         * We can use the member names to construct:
         *
         *     "Address.City"
         *
         * EF Core then treats this as one Include path.
         */

        foreach (var thenInclude in specification.ThenIncludes)
        {
            var currentPropertyName =
                GetMemberName(thenInclude.LambdaExpression);

            var previousPropertyName =
                GetPreviousPropertyName(
                    thenInclude,
                    expressionPropertyNames);

            var includePath =
                $"{previousPropertyName}.{currentPropertyName}";

            query = query.Include(includePath);

            /*
             * Store the incoming ThenInclude expression as well.
             *
             * This allows the next ThenInclude to continue the chain.
             *
             * Example:
             *
             * Customer
             *    └── Address
             *          └── City
             *                └── Country
             *
             * Address expression -> Address
             * City expression    -> City
             * Country expression -> Country
             */

            expressionPropertyNames[
                thenInclude.LambdaExpression] =
                includePath;
        }

        return query;
    }

    private static string GetPreviousPropertyName<TEntity>(
        ThenIncludesExpressionInfo<TEntity> thenInclude,
        Dictionary<LambdaExpression, string> expressionPropertyNames)
        where TEntity : BaseEntity
    {
        if (expressionPropertyNames.TryGetValue(
                thenInclude.PreviousExpression,
                out var propertyName))
            return propertyName;

        throw new InvalidOperationException(
            "The previous include expression was not found.");
    }

    private static string GetMemberName<TEntity>(
        Expression<Func<TEntity, object>> expression)
        where TEntity : BaseEntity
        => GetMemberName(
            (LambdaExpression)expression);

    private static string GetMemberName(
        LambdaExpression expression)
    {
        Expression body = expression.Body;

        /*
         * Because the expression is:
         *
         * x => x.Address
         *
         * the body is normally MemberExpression.
         *
         * However, because our Include expression is:
         *
         * Expression<Func<TEntity, object>>
         *
         * value types may be wrapped inside:
         *
         * Convert(...)
         *
         * Therefore unwrap Convert first.
         */

        if (body is UnaryExpression
            {
                NodeType: ExpressionType.Convert
            } unaryExpression)
        {
            body = unaryExpression.Operand;
        }

        if (body is not MemberExpression memberExpression)
        {
            throw new InvalidOperationException(
                $"Expression '{expression}' must be a member expression.");
        }

        return memberExpression.Member.Name;
    }

    // ============================================================
    // ORDERING
    // ============================================================

    private static IQueryable<TEntity> ApplyOrdering<TEntity>(
        IQueryable<TEntity> query,
        ISpecifications<TEntity> specification)
        where TEntity : BaseEntity
    {
        if (specification.OrderByThenByExpressions.Count == 0)
        {
            return query;
        }

        var isFirstOrder = true;

        foreach (var orderExpression
                 in specification.OrderByThenByExpressions)
        {
            query = ApplyOrder(
                query,
                orderExpression,
                isFirstOrder);

            isFirstOrder = false;
        }

        return query;
    }

    private static IQueryable<TEntity> ApplyOrder<TEntity>(
        IQueryable<TEntity> query,
        OrderExpressionInfo<TEntity> orderExpression,
        bool isFirstOrder)
        where TEntity : BaseEntity
    {
        /*
         * First expression:
         *
         * OrderBy
         * OrderByDescending
         *
         * Subsequent expressions:
         *
         * ThenBy
         * ThenByDescending
         */

        if (isFirstOrder)
        {
            return orderExpression.OrderType switch
            {
                OrderType.Ascending =>
                    query.OrderBy(
                        orderExpression.KeySelector),

                OrderType.Descending =>
                    query.OrderByDescending(
                        orderExpression.KeySelector),

                _ => throw new ArgumentOutOfRangeException(
                    nameof(orderExpression.OrderType))
            };
        }

        var orderedQuery =
            (IOrderedQueryable<TEntity>)query;

        return orderExpression.OrderType switch
        {
            OrderType.Ascending =>
                orderedQuery.ThenBy(
                    orderExpression.KeySelector),

            OrderType.Descending =>
                orderedQuery.ThenByDescending(
                    orderExpression.KeySelector),

            _ => throw new ArgumentOutOfRangeException(
                nameof(orderExpression.OrderType))
        };
    }

    // ============================================================
    // TRACKING
    // ============================================================

    private static IQueryable<TEntity> ApplyTracking<TEntity>(
        IQueryable<TEntity> query,
        ISpecifications<TEntity> specification)
        where TEntity : BaseEntity
    {
        if (!specification.IsTrackingEnabled)
            query = query.AsNoTracking();

        return query;
    }

    // ============================================================
    // PAGINATION
    // ============================================================

    private static IQueryable<TEntity> ApplyPagination<TEntity>(
        IQueryable<TEntity> query,
        ISpecifications<TEntity> specification)
        where TEntity : BaseEntity
    {
        if (!specification.IsPaginatedEnabled)
            return query;

        if (specification.Skip is int skip)
            query = query.Skip(skip);

        if (specification.Take is int take)
            query = query.Take(take);

        return query;
    }

    // ============================================================
    // PROJECTION
    // ============================================================

    private static IQueryable<TResult> ApplyProjection<TEntity, TResult>(
        IQueryable<TEntity> query,
        ISpecifications<TEntity, TResult> specification)
        where TEntity : BaseEntity
    {
        if (specification.Selector is not null)
            return query.Select(
                specification.Selector);

        if (specification.ManySelector is not null)
            return query.SelectMany(
                specification.ManySelector);

        throw new InvalidOperationException(
            $"Specification '{specification.GetType().Name}' " +
            "does not contain a projection.");
    }
}