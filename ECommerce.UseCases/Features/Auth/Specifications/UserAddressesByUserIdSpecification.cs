using ECommerce.Domain.Entities.UserModule;
using ECommerce.UseCases.Features.Auth.DTOs;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Identity.Specifications;

public sealed class UserAddressesByUserIdSpecification
    : Specification<UserAddress, UserAddressResponse>
{
    public UserAddressesByUserIdSpecification(Guid userId)
    {
        Query
            .Where(a => a.ApplicationUserId == userId)
            .OrderByDescending(a => a.CreatedAt)
            .ThenBy(a => a.Label);

        Query.Select(a => new UserAddressResponse(
                a.Id,
                a.Label,
                a.RecipientFirstName,
                a.RecipientLastName,
                a.Phone,
                a.Country,
                a.City,
                a.Street,
                a.IsDefault));
    }
}
