using ECommerce.Domain.Entities.OrderModule;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.Orders.Specifications
{
    internal class OrderByPaymentIntentIdSpecification
        : Specification<Order>
    {
        public OrderByPaymentIntentIdSpecification(string paymentIntentId, bool tracking = false)
        {
            var query = Query.Where(o => o.PaymentIntentId == paymentIntentId);
            if (tracking)
                query.AsTracking();
            else
                query.AsNoTracking();
        }
    }
}
