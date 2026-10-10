using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.Auth.DTOs;
using MediatR;

namespace ECommerce.UseCases.Identity.Queries.GetUserAddresses;

public sealed record GetUserAddressesQuery
    : IRequest<Result<IReadOnlyList<UserAddressResponse>>>;
