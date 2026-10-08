using ECommerce.UseCases.Specifications;

namespace ECommerce.Infrastructure.Email.Specifications
{
    public sealed class GetVerificationCodeByUserIdSpecification :
        Specification<EmailVerificationCode>
    {
        public GetVerificationCodeByUserIdSpecification(Guid userId)
            => Query.Where(x => x.ApplicationUserId == userId)
                    .OrderByDescending(x => x.ExpirationTime)
                    .Take(1);
    }
}