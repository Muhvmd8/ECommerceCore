using ECommerce.API.Models;
using ECommerce.UseCases.Features.Auth.Commands.ChangePassword;
using ECommerce.UseCases.Features.Auth.Commands.Login;
using ECommerce.UseCases.Features.Auth.Commands.RefreshToken;
using ECommerce.UseCases.Features.Auth.Commands.Register;
using ECommerce.UseCases.Features.Auth.Commands.VerifyRegistrationOtp;
using ECommerce.UseCases.Features.Auth.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    public class AuthController(IMediator mediator) : ApiController
    {
        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse<RegisterResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ApiResponse<RegisterResponse>>> Register(
            RegisterCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = await mediator.Send(command, cancellationToken);
            return FromResult(result, "Registration Completed Successfully");
        }

        [HttpPost("verify-registration-otp")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<object>>> VerifyRegistrationOtp(
            VerifyRegistrationOtpCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = await mediator.Send(command, cancellationToken);
            return Success<object>(result, "Email Verified Successfully");
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<LoginResponse>>> Login(
            LoginCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = await mediator.Send(command, cancellationToken);
            return FromResult(result, "Login Successfully");
        }

        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(ApiResponse<RefreshTokenResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<RefreshTokenResponse>>> RefreshToken(
            RefreshTokenCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = await mediator.Send(command, cancellationToken);
            return FromResult(result, "Token Refreshed Successfully");
        }

        [HttpPost("change-password")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<object>>> ChangePassword(
            ChangePasswordCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = await mediator.Send(command, cancellationToken);
            return FromResult<object>(result, "Password Changed Successfully");
        }
    }
}
