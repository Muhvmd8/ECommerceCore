using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.Auth.DTOs;
using MediatR;

namespace ECommerce.UseCases.Identity.Queries.GetCurrentUser;

public sealed record GetCurrentUserQuery : IRequest<Result<UserProfileResponse>>;
