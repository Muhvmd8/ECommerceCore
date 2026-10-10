using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.Auth.DTOs;
using MediatR;

namespace ECommerce.UseCases.Features.Auth.Commands.AddUserAddress;

public sealed record AddUserAddressCommand(
    string Label,
    string RecipientFirstName,
    string RecipientLastName,
    string PhoneNumber,
    string Country,
    string City,
    string Street,
    string PostalCode,
    bool IsDefaultShipping = false,
    bool IsDefaultBilling = false) : IRequest<Result<UserAddressResponse>>;
