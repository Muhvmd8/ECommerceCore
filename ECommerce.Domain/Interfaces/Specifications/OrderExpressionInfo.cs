using ECommerce.Domain.Entities.Common;
using System.Linq.Expressions;
namespace ECommerce.Domain.Interfaces.Specifications;
public record OrderExpressionInfo<TEntity>
    where TEntity : BaseEntity
{
    public Expression<Func<TEntity, object?>> KeySelector { get; set; } = default!;
    public OrderType OrderType { get; set; }
};
public enum OrderType
{
    Ascending,
    Descending,
    ThenAscending,
    ThenDescending
}