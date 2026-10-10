using ECommerce.Domain.Entities.UserModule;
using ECommerce.Domain.Errors;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Abstractions;
using ECommerce.UseCases.Features.Auth.DTOs;
using ECommerce.UseCases.Identity.Specifications;
using MediatR;

namespace ECommerce.UseCases.Identity.Queries.GetUserAddresses;

public sealed class GetUserAddressesQueryHandler(
    ICurrentUserService currentUser,
    IReadRepository<UserAddress> addressRepository)
    : IRequestHandler<GetUserAddressesQuery, Result<IReadOnlyList<UserAddressResponse>>>
{
    public async Task<Result<IReadOnlyList<UserAddressResponse>>> Handle(
        GetUserAddressesQuery request,
        CancellationToken cancellationToken)
    {
        if (currentUser.UserId is null)
            return Result<IReadOnlyList<UserAddressResponse>>.Failure(IdentityErrors.InvalidCredentials);

        var addresses = await addressRepository.ListAsync(
            new UserAddressesByUserIdSpecification(currentUser.UserId.Value),
            cancellationToken);

        return Result<IReadOnlyList<UserAddressResponse>>.Success(addresses);
    }
}
