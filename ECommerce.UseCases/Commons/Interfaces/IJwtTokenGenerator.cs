namespace ECommerce.UseCases.Commons.Interfaces
{
    public record AccessTokenResult(string Token, DateTimeOffset ExpiresAtUtc);
    public interface IJwtTokenGenerator
    {
        AccessTokenResult GenerateToken(Guid userId, string email, string? displayName, IEnumerable<string> roles);
    }
}