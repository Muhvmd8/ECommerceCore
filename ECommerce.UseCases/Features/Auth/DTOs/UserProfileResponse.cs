namespace ECommerce.UseCases.Features.Auth.DTOs
{
    public sealed record UserProfileResponse(
        Guid UserId,
        string Email,
        string? DisplayName);
}
