using ECommerce.Domain.Entities.Auth;
using ECommerce.UseCases.Specifications;
namespace ECommerce.Infrastructure.Identity.Specifications
{
    public sealed class GetRefreshTokenSpecification
        : Specification<RefreshToken>
    {
        public GetRefreshTokenSpecification(string token)
        {
            Query.Where(x => x.Token == token);
        }
    }
}
