using ECommerce.Domain.Entities.EmailVerificationCodeModule;
using ECommerce.Domain.Errors;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using ECommerce.Infrastructure.Identity;
using ECommerce.UseCases.Abstractions;
using ECommerce.UseCases.Commons.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace ECommerce.Infrastructure.Email;

public sealed class EmailVerificationService(
    IEmailStore emailStore,
    IMailingService mailingService,
    UserManager<ApplicationUser> userManager,
    IHostEnvironment environment,
    IOptions<OtpSettings> otpSettings)
    : IEmailVerificationService
{
    private readonly OtpSettings _otpSettings = otpSettings.Value;
    public async Task<Result> SendRegistrationCodeAsync(
        Guid userId,
        string email,
        CancellationToken cancellationToken = default)
    {
        return await _SendCodeAsync(
            userId,
            email,
            EmailVerificationPurpose.Registration,
            "VerificationOtp.html",
            "Email Verification Code",
            cancellationToken);
    }

    public async Task<Result> VerifyRegistrationCodeAsync(
        string email,
        string code,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
            return Result.Failure(IdentityErrors.UserNotFound);

        if (user.EmailConfirmed)
            return Result.Failure(
                IdentityErrors.EmailAlreadyConfirmed);

        var verification = await _GetValidCodeAsync(
            user.Id,
            EmailVerificationPurpose.Registration,
            code,
            cancellationToken);

        if (verification is null)
            return Result.Failure(
                IdentityErrors.InvalidVerification);

        user.EmailConfirmed = true;

        var updateResult = await userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
            return Result.Failure(
                IdentityErrors.EmailConfirmationFailed);

        await emailStore.RemoveAsync(
            user.Id,
            EmailVerificationPurpose.Registration,
            cancellationToken);

        return Result.Success();
    }

    public async Task<Result> SendPasswordResetCodeAsync(
        Guid userId,
        string email,
        CancellationToken cancellationToken = default)
    {
        return await _SendCodeAsync(
            userId,
            email,
            EmailVerificationPurpose.PasswordReset,
            "ResetPasswordOtp.html",
            "Password Reset Code",
            cancellationToken,
            isPasswordReset: true);
    }

    public async Task<Result<string>> VerifyPasswordResetCodeAsync(
        Guid userId,
        string code,
        CancellationToken cancellationToken = default)
    {
        var verification = await _GetValidCodeAsync(
            userId,
            EmailVerificationPurpose.PasswordReset,
            code,
            cancellationToken);

        if (verification is null)
            return Result<string>.Failure(
                IdentityErrors.InvalidVerification);

        var user = await userManager.FindByIdAsync(
            userId.ToString());

        if (user is null)
            return Result<string>.Failure(
                IdentityErrors.UserNotFound);

        await emailStore.RemoveAsync(
            userId,
            EmailVerificationPurpose.PasswordReset,
            cancellationToken);

        var resetToken =
            await userManager.GeneratePasswordResetTokenAsync(user);

        var encodedToken = WebEncoders.Base64UrlEncode(
            Encoding.UTF8.GetBytes(resetToken));

        return Result<string>.Success(encodedToken);
    }

    private async Task<Result> _SendCodeAsync(
        Guid userId,
        string email,
        EmailVerificationPurpose purpose,
        string templateName,
        string subject,
        CancellationToken cancellationToken,
        bool isPasswordReset = false)
    {
        var user = await userManager.FindByIdAsync(
            userId.ToString());

        if (user is null)
            return Result.Failure(IdentityErrors.UserNotFound);

        var code = _GenerateOtp(_otpSettings.Length);

        var verificationCode = new EmailVerificationCode
        {
            UserId = userId,
            Code = code,
            Purpose = purpose,
            ExpirationTimeUtc = DateTimeOffset.UtcNow
                .AddMinutes(_otpSettings.ExpirationMinutes)
        };

        await emailStore.SaveAsync(
            verificationCode,
            cancellationToken);

        var bodyResult = await _LoadTemplateAsync(
            templateName,
            cancellationToken);

        if (bodyResult.IsFailure)
        {
            await emailStore.RemoveAsync(
                userId,
                purpose,
                cancellationToken);

            return Result.Failure(bodyResult.Error!);
        }

        var body = bodyResult.Value!
            .Replace("{{otp_code}}", code);

        if (isPasswordReset)
            body = body.Replace("{{Logo}}", "logo");

        await mailingService.SendEmailAsync(
            email,
            subject,
            body,
            null,
            cancellationToken);

        return Result.Success();
    }

    private async Task<EmailVerificationCode?> _GetValidCodeAsync(
        Guid userId,
        EmailVerificationPurpose purpose,
        string code,
        CancellationToken cancellationToken)
    {
        var verification = await emailStore.GetAsync(
            userId,
            purpose,
            cancellationToken);

        if (verification is null)
            return null;

        if (verification.ExpirationTimeUtc <=
            DateTimeOffset.UtcNow)
        {
            await emailStore.RemoveAsync(
                userId,
                purpose,
                cancellationToken);

            return null;
        }

        if (!string.Equals(
            verification.Code,
            code,
            StringComparison.Ordinal))
        {
            return null;
        }

        return verification;
    }

    private async Task<Result<string>> _LoadTemplateAsync(
        string fileName,
        CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(
            environment.ContentRootPath,
            "wwwroot",
            "HTML",
            fileName);

        if (!File.Exists(filePath))
            return Result<string>.Failure(
                EmailErrors.TemplateNotFound);

        var body = await File.ReadAllTextAsync(
            filePath,
            cancellationToken);

        return Result<string>.Success(body);
    }

    private static string _GenerateOtp(int length)
    {
        if (length is < 4 or > 8)
        {
            throw new InvalidOperationException(
                "OTP length must be between 4 and 8.");
        }

        var min = (int)Math.Pow(10, length - 1);
        var max = (int)Math.Pow(10, length);

        return RandomNumberGenerator
            .GetInt32(min, max)
            .ToString();
    }
}
