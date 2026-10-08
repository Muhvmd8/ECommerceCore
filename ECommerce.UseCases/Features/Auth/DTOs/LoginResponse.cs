namespace ECommerce.UseCases.Features.Auth.DTOs
{
    public sealed record LoginResponse(
        string AccessToken,
        string RefreshToken,
        DateTime AccessTokenExpiresAtUtc,
        DateTime RefreshTokenExpiresAtUtc);
}