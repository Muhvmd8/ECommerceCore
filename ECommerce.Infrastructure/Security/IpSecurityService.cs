using ECommerce.Domain.Entities.Security;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Abstractions;
using ECommerce.UseCases.Specifications.Specifications.Security;
using Microsoft.Extensions.Logging;

namespace ECommerce.Infrastructure.Security
{
    public sealed class IpSecurityService : IIpSecurityService
    {
        private readonly IReadRepository<LoginAudit> _loginAuditRepository;
        private readonly IReadRepository<BlockedIp> _blockedIpRepository;
        private readonly ILogger<IpSecurityService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public IpSecurityService(
            IReadRepository<LoginAudit> loginAuditRepository,
            IReadRepository<BlockedIp> blockedIpRepository,
            ILogger<IpSecurityService> logger,
            IUnitOfWork unitOfWork)
        {
            _loginAuditRepository = loginAuditRepository;
            _blockedIpRepository = blockedIpRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> TooManyFailedLogins(
            string ip,
            int maxAttempts,
            TimeSpan window,
            CancellationToken cancellationToken = default)
        {
            var cutoff = DateTime.UtcNow - window;

            var specification =
                new FailedLoginAttemptsSpecification(ip, cutoff);

            var failedAttempts = await _loginAuditRepository.CountAsync(
                specification,
                cancellationToken);

            return failedAttempts >= maxAttempts;
        }

        public async Task BlockIpAsync(
            string ip,
            string reason,
            TimeSpan duration,
            CancellationToken cancellationToken = default)
        {
            var repository =
                _unitOfWork.GetRepository<BlockedIp>();

            var blockedIp = new BlockedIp
            {
                IpAddress = ip,
                Reason = reason,
                ExpiresAt = DateTime.UtcNow.Add(duration)
            };

            await repository.AddAsync(
                blockedIp,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            _logger.LogWarning(
                "IP {IpAddress} has been blocked. Reason: {Reason}, Duration: {Duration}",
                ip,
                reason,
                duration);
        }

        public async Task<bool> IsIpBlockedAsync(
            string ip,
            CancellationToken cancellationToken = default)
        {
            var specification =
                new ActiveBlockedIpSpecification(ip, DateTime.UtcNow);

            var blockedIp = await _blockedIpRepository.FirstOrDefaultAsync(
                specification,
                cancellationToken);

            return blockedIp is not null;
        }

        public bool ContainsMaliciousPayload(string? body)
        {
            if (string.IsNullOrWhiteSpace(body))
                return false;

            body = body.ToLowerInvariant();

            return ContainsXss(body)
                || ContainsSqlInjection(body);
        }

        private static bool ContainsXss(string body)
        {
            string[] xssPatterns =
            [
                "<script",
                "</script",
                "javascript:",
                "onerror=",
                "onload=",
                "onmouseover=",
                "onclick=",
                "eval(",
                "document.cookie",
                "window.location"
            ];

            return xssPatterns.Any(body.Contains);
        }

        private static bool ContainsSqlInjection(string body)
        {
            string[] sqlPatterns =
            [
                "' or 1=1",
                "\" or 1=1",
                "--",
                "/*",
                "*/",
                "union select",
                "drop table",
                "insert into",
                "update set",
                "delete from",
                "xp_cmdshell",
                "exec("
            ];

            return sqlPatterns.Any(body.Contains);
        }
    }
}
