using ECommerce.UseCases.Specifications;

namespace ECommerce.Infrastructure.Email.Specifications
{
    public sealed class GetVerificationCodeByUserIdAndCodeSpecification
        : Specification<EmailVerificationCode>
    {
        public GetVerificationCodeByUserIdAndCodeSpecification(Guid userId, string code)
            => Query.Where(x => x.ApplicationUserId == userId && x.Code == code);
    }
}