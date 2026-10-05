using ECommerce.Domain.Entities.Security;

namespace ECommerce.UseCases.Specifications.Specifications.Security
{
    public sealed class FailedLoginAttemptsSpecification
        : Specification<LoginAudit>
    {
        public FailedLoginAttemptsSpecification(
            string ipAddress,
            DateTime cutoff)
        {
            AddCriteria(x =>
                x.IpAddress == ipAddress &&
                x.Status == "Failed" &&
                x.CreatedAt >= cutoff);
        }
    }
}
