using ECommerce.Domain.Entities.Common;
using System.Linq.Expressions;
namespace ECommerce.Domain.Interfaces.Specifications;
public sealed record ThenIncludesExpressionInfo<TEntity>
    where TEntity : BaseEntity
{
    public LambdaExpression LambdaExpression { get; set; } = default!;
    public LambdaExpression PreviousExpression { get; set; } = default!;
};