using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.Auth.DTOs;

namespace ECommerce.UseCases.Abstractions
{
    public interface IIdentityService
    {
        Task<Result<AuthUserSnapshot>> CreateUserAsync(string email, string password, string? displayName, CancellationToken cancellationToken = default);
        Task<Result<AuthUserSnapshot>> ValidateCredentialsAsync(string email, string password, CancellationToken cancellationToken = default);
        Task<Result<AuthUserSnapshot>> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<Result<AuthUserSnapshot>> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<AuthUserSnapshot>> UpdateProfileAsync(Guid userId, string? displayName, CancellationToken cancellationToken = default);
        Task<Result> ConfirmEmailAsync(Guid userId, string verificationCode, CancellationToken cancellationToken = default);
        Task<bool> IsEmailConfirmedAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<Result> ResendVerificationAsync(string email, CancellationToken cancellationToken = default);
        Task<Result> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default);
        Task<Result> AddToRoleAsync(Guid userId, string role, CancellationToken cancellationToken = default);
        Task<Result> RemoveFromRoleAsync(Guid userId, string role, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<string>>> GetRolesAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<Result> RoleExistsAsync(string role, CancellationToken cancellationToken = default);
    }
}
