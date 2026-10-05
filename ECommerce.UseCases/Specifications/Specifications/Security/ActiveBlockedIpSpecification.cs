using ECommerce.Domain.Entities.Security;

namespace ECommerce.UseCases.Specifications.Specifications.Security
{
    public sealed class ActiveBlockedIpSpecification
        : Specification<BlockedIp>
    {
        public ActiveBlockedIpSpecification(
            string ipAddress,
            DateTime now)
        {
            AddCriteria(x =>
                x.IpAddress == ipAddress &&
                (x.ExpiresAt == null || x.ExpiresAt > now));
        }
    }
}
