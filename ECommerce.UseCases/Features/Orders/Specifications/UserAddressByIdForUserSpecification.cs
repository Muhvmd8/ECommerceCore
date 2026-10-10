using ECommerce.Domain.Entities.UserModule;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.Orders.Specifications;

public sealed class UserAddressByIdForUserSpecification
    : Specification<UserAddress>
{
    public UserAddressByIdForUserSpecification(Guid addressId, Guid userId)
    {
        Query.Where(a => a.Id == addressId && a.ApplicationUserId == userId);
    }
}
