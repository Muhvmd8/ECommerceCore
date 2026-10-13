using ECommerce.Domain.Constants;
using ECommerce.Domain.Errors;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Abstractions;
using ECommerce.UseCases.Features.Auth.DTOs;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Infrastructure.Identity.Services
{
    public sealed class IdentityService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IEmailVerificationService emailVerificationService)
        : IIdentityService
    {

        public async Task<Result<AuthUserSnapshot>> CreateUserAsync(
            string email,
            string password,
            string? displayName,
            CancellationToken cancellationToken = default)
        {
            email = email.Trim();

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                DisplayName = displayName?.Trim() ?? string.Empty
            };

            var createResult = await userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
            {
                if (createResult.Errors.Any(e =>
                    e.Code is "DuplicateEmail" or "DuplicateUserName"))
                {
                    return Result<AuthUserSnapshot>.Failure(IdentityErrors.EmailAlreadyExists);
                }

                var message = string.Join(
                    ", ",
                    createResult.Errors.Select(e => e.Description));

                return Result<AuthUserSnapshot>.Failure(
                    IdentityErrors.CreateFailed(message));
            }

            var roleResult = await userManager.AddToRoleAsync(user, Roles.User);

            if (!roleResult.Succeeded)
            {
                // Compensate for a partially completed registration.
                await userManager.DeleteAsync(user);
                var message = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                return Result<AuthUserSnapshot>.Failure(IdentityErrors.CreateFailed(message));
            }

            return Result<AuthUserSnapshot>.Success(
                new AuthUserSnapshot(
                    user.Id,
                    user.Email!,
                    user.DisplayName,
                    user.EmailConfirmed));
        }


        public async Task<Result<AuthUserSnapshot>> ValidateCredentialsAsync(
            string email,
            string password,
            CancellationToken cancellationToken = default)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user is null)
                return Result<AuthUserSnapshot>.Failure(IdentityErrors.InvalidCredentials);

            if (!user.EmailConfirmed)
                return Result<AuthUserSnapshot>.Failure(IdentityErrors.EmailNotConfirmed);

            var passwordValid = await userManager.CheckPasswordAsync(user, password);

            if (!passwordValid)
                return Result<AuthUserSnapshot>.Failure(IdentityErrors.InvalidCredentials);

            var snapshot = new AuthUserSnapshot(
                user.Id,
                user.Email!,
                user.DisplayName,
                user.EmailConfirmed
            );

            return Result<AuthUserSnapshot>.Success(snapshot);
        }

        public async Task<Result<AuthUserSnapshot>> GetUserByEmailAsync(
            string email,
            CancellationToken cancellationToken = default)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
                return Result<AuthUserSnapshot>.Failure(IdentityErrors.UserNotFound);

            var snapshot = new AuthUserSnapshot(
                user.Id,
                user.Email!,
                user.DisplayName,
                user.EmailConfirmed
            );
            return Result<AuthUserSnapshot>.Success(snapshot);
        }

        public async Task<Result<AuthUserSnapshot>> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user is null)
                return Result<AuthUserSnapshot>.Failure(IdentityErrors.UserNotFound);

            var snapshot = new AuthUserSnapshot(
                user.Id,
                user.Email!,
                user.DisplayName,
                user.EmailConfirmed
            );
            return Result<AuthUserSnapshot>.Success(snapshot);
        }

        public async Task<Result<AuthUserSnapshot>> UpdateProfileAsync(Guid userId, string? displayName, CancellationToken cancellationToken = default)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                return Result<AuthUserSnapshot>.Failure(IdentityErrors.InvalidCredentials);

            user.DisplayName = string.IsNullOrWhiteSpace(displayName) ? user.DisplayName : displayName.Trim();

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var message = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<AuthUserSnapshot>.Failure(IdentityErrors.CreateFailed(message));
            }

            var snapshot = new AuthUserSnapshot(
                user.Id,
                user.Email!,
                user.DisplayName,
                user.EmailConfirmed
            );

            return Result<AuthUserSnapshot>.Success(snapshot);
        }

        public async Task<bool> IsEmailConfirmedAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                return false;
            return user.EmailConfirmed;
        }

        public async Task<Result> ConfirmEmailAsync(
            Guid userId,
            string verificationCode,
            CancellationToken cancellationToken = default)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                return Result.Failure(IdentityErrors.UserNotFound);

            if (user.EmailConfirmed)
                return Result.Failure(IdentityErrors.EmailAlreadyConfirmed);

            var result = await userManager.ConfirmEmailAsync(user, verificationCode);

            if (!result.Succeeded)
                return Result.Failure(IdentityErrors.InvalidVerification);

            return Result.Success();
        }

        public async Task<Result> ResendVerificationAsync(
            string email,
            CancellationToken cancellationToken = default)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user is null)
                return Result.Failure(IdentityErrors.UserNotFound);

            if (user.EmailConfirmed)
                return Result.Failure(IdentityErrors.EmailAlreadyConfirmed);

            return await emailVerificationService.SendRegistrationCodeAsync(
                user.Id,
                email,
                cancellationToken);
        }

        public async Task<Result> ChangePasswordAsync(
            Guid userId,
            string currentPassword,
            string newPassword,
            CancellationToken cancellationToken = default)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                return Result.Failure(IdentityErrors.UserNotFound);

            var result = await userManager.ChangePasswordAsync(
                user,
                currentPassword,
                newPassword);

            if (!result.Succeeded)
                return Result.Failure(IdentityErrors.PasswordChangeFailed);

            return Result.Success();
        }

        public async Task<Result> AddToRoleAsync(
            Guid userId,
            string role,
            CancellationToken cancellationToken = default)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                return Result.Failure(IdentityErrors.UserNotFound);

            var roleExists =
                await roleManager.RoleExistsAsync(role);

            if (!roleExists)
                return Result.Failure(IdentityErrors.RoleNotFound);

            var result = await userManager.AddToRoleAsync(user, role);

            if (!result.Succeeded)
                return Result.Failure(IdentityErrors.RoleAssignmentFailed);

            return Result.Success();
        }

        public async Task<Result> RemoveFromRoleAsync(
            Guid userId,
            string role,
            CancellationToken cancellationToken = default)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                return Result.Failure(IdentityErrors.UserNotFound);

            var roleExists =
                await roleManager.RoleExistsAsync(role);

            if (!roleExists)
                return Result.Failure(IdentityErrors.RoleNotFound);

            var result = await userManager.RemoveFromRoleAsync(user, role);
            if (!result.Succeeded)
                return Result.Failure(IdentityErrors.RoleAssignmentFailed);

            return Result.Success();
        }

        public async Task<Result<IEnumerable<string>>> GetRolesAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                return Result<IEnumerable<string>>.Failure(IdentityErrors.UserNotFound);

            var roles = await userManager.GetRolesAsync(user);

            return Result<IEnumerable<string>>.Success(roles.ToList());
        }

        public async Task<Result> RoleExistsAsync(
            string role,
            CancellationToken cancellationToken = default)
        {
            var exists = await roleManager.RoleExistsAsync(role);

            if (!exists)
                return Result.Failure(IdentityErrors.RoleNotFound);
            return Result.Success();
        }
    }
}
