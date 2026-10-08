namespace ECommerce.UseCases.Features.Auth.DTOs
{
    public record AuthUserSnapshot(
        Guid UserId,
        string Email,
        string DisplayName,
        bool IsEmailConfirmed
    );
}
