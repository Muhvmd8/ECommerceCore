using ECommerce.Domain.Entities.UserModule;
using ECommerce.Domain.Errors;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Abstractions;
using ECommerce.UseCases.Features.Auth.DTOs;
using MediatR;

namespace ECommerce.UseCases.Features.Auth.Commands.AddUserAddress;

public sealed class AddUserAddressCommandHandler(
    ICurrentUserService currentUser,
    IRepository<UserAddress> addressRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<AddUserAddressCommand, Result<UserAddressResponse>>
{
    public async Task<Result<UserAddressResponse>> Handle(
        AddUserAddressCommand request,
        CancellationToken cancellationToken)
    {
        if (currentUser.UserId is null)
            return Result<UserAddressResponse>.Failure(IdentityErrors.InvalidCredentials);

        var createResult = UserAddress.Create(
            Guid.NewGuid(),
            currentUser.UserId.Value,
            request.Label,
            request.PhoneNumber,
            request.Country,
            request.City,
            request.RecipientFirstName,
            request.RecipientLastName,
            request.Street,
            request.IsDefaultShipping);

        if (createResult.IsFailure)
            return Result<UserAddressResponse>.Failure(createResult.Error!);

        var address = createResult.Value;
        await addressRepository.AddAsync(address, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<UserAddressResponse>.Success(new UserAddressResponse(
            address.Id,
            address.Label,
            address.RecipientFirstName,
            address.RecipientLastName,
            address.Phone,
            address.Country,
            address.City,
            address.Street,
            address.IsDefault));
    }
}
