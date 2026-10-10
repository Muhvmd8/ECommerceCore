namespace ECommerce.UseCases.Abstractions
{
    public record AccessTokenResult(string Token, string JwtId, DateTime ExpiresAtUtc);
    public interface IJwtTokenGenerator
    {
        AccessTokenResult GenerateToken(Guid userId, string email, string? displayName, IEnumerable<string> roles);
    }
}