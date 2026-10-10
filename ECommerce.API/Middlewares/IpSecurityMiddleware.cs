using ECommerce.API.Factories;
using ECommerce.Domain.Errors;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Abstractions;

namespace ECommerce.API.Middlewares
{
    public sealed class IpSecurityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<IpSecurityMiddleware> _logger;

        public IpSecurityMiddleware(
            RequestDelegate next,
            ILogger<IpSecurityMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(
            HttpContext context,
            IIpSecurityService security)
        {
            var cancellationToken = context.RequestAborted;

            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            // 1. Check if IP is already blocked
            if (await security.IsIpBlockedAsync(ip, cancellationToken))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                var result = Result.Failure(SecurityErrors.IpBlocked);
                var problem = ApiResponseFactory.Problem(result, context.TraceIdentifier);
                context.Response.StatusCode = problem.StatusCode ?? StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(problem, cancellationToken);
                return;
            }

            // 2. Read request body when available
            string? body = null;

            if (context.Request.ContentLength is > 0)
            {
                context.Request.EnableBuffering();

                using var reader = new StreamReader(
                    context.Request.Body,
                    leaveOpen: true);

                body = await reader.ReadToEndAsync(
                    cancellationToken);

                context.Request.Body.Position = 0;
            }

            // 3. Check malicious payload
            if (security.ContainsMaliciousPayload(body))
            {
                await security.BlockIpAsync(
                    ip,
                    "Malicious Payload Detected",
                    TimeSpan.FromMinutes(60),
                    cancellationToken);

                context.Response.StatusCode =
                    StatusCodes.Status403Forbidden;

                var result = Result.Failure(SecurityErrors.MaliciousPayloadDetected);

                var problem = ApiResponseFactory.Problem(
                    result,
                    context.TraceIdentifier);

                context.Response.StatusCode = problem.StatusCode ?? StatusCodes.Status403Forbidden;

                await context.Response.WriteAsJsonAsync(
                    problem,
                    cancellationToken);

                return;
            }

            // 4. Continue request pipeline
            await _next(context);

            // 5. Check brute-force attempts
            if (await security.TooManyFailedLogins(
                ip,
                maxAttempts: 5,
                window: TimeSpan.FromMinutes(5),
                cancellationToken))
            {
                await security.BlockIpAsync(
                    ip,
                    "Brute Force Detected",
                    TimeSpan.FromMinutes(30),
                    cancellationToken);

                _logger.LogWarning(
                    "Brute force detected for IP {IpAddress}",
                    ip);
            }

            // if (riskDetected)
            // {
            //     db.LoginAudits.Add(new LoginAudit
            //     {
            //         Username = context.User?.Identity?.Name ?? "Anonymous",
            //         IpAddress = ip,
            //         Endpoint = context.Request.Path,
            //         Status = context.Response.StatusCode < 400 ? "Success" : "Failed",
            //         RequestBody = body
            //     });
            // 
            //     await db.SaveChangesAsync();
            // }
        }
    }
}
