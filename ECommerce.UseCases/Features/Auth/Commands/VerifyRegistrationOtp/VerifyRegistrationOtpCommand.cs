using ECommerce.Domain.Shared;
using MediatR;

namespace ECommerce.UseCases.Features.Auth.Commands.VerifyRegistrationOtp
{
    public sealed record VerifyRegistrationOtpCommand(string Email, string Otp)
        : IRequest<Result>;
}