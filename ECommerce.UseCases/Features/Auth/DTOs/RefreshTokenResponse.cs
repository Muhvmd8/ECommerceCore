namespace ECommerce.UseCases.Features.Auth.DTOs
{
    public sealed record RefreshTokenResponse(
        string AccessToken,
        string RefreshToken,
        DateTime AccessTokenExpiresAtUtc,
        DateTime RefreshTokenExpiresAtUtc);
}
