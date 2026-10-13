namespace ECommerce.UseCases.Commons.DTOs
{
    public sealed record RefreshTokenResult(string Token, DateTime ExpiresAtUtc);
}