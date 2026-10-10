using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.Auth.DTOs;
using MediatR;

namespace ECommerce.UseCases.Features.Auth.Commands.UpdateUserProfile;

public sealed record UpdateUserProfileCommand(string? DisplayName)
    : IRequest<Result<UserProfileResponse>>;
