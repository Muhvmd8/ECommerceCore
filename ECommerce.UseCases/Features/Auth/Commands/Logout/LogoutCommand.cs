using ECommerce.Domain.Shared;
using MediatR;

namespace ECommerce.UseCases.Features.Auth.Commands.Logout;

public sealed record LogoutCommand(
    string RefreshToken) : IRequest<Result>;
