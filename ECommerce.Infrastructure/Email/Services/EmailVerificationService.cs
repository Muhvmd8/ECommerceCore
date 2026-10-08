using ECommerce.Domain.Errors;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using ECommerce.Infrastructure.Email.Specifications;
using ECommerce.Infrastructure.Identity;
using ECommerce.UseCases.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Hosting;
using System.Text;

namespace ECommerce.Infrastructure.Email.Services;

public sealed class EmailVerificationService(
    IUnitOfWork unitOfWork,
    IMailingService mailingService,
    UserManager<ApplicationUser> userManager,
    IHostEnvironment environment)
    : IEmailVerificationService
{
    private const int OtpExpirationMinutes = 10;

    public async Task<Result> SendRegistrationCodeAsync(
        Guid userId,
        string email,
        CancellationToken cancellationToken = default)
    {
        var code = GenerateOtp();

        var saveResult = await SaveCodeAsync(userId, code, cancellationToken);

        if (saveResult.IsFailure)
            return saveResult;

        var bodyResult = await LoadTemplateAsync("VerificationOtp.html", cancellationToken);

        if (bodyResult.IsFailure)
            return Result.Failure(bodyResult.Error!);

        var body = bodyResult.Value
            .Replace("{{otp_code}}", code);

        await mailingService.SendEmailAsync(
            email,
            "Email Verification Code",
            body,
            null,
            cancellationToken);

        return Result.Success();
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
            return Result.Failure(IdentityErrors.EmailAlreadyConfirmed);

        var verification = await GetVerificationCodeAsync(
            user.Id,
            code,
            cancellationToken);

        if (verification is null)
            return Result.Failure(IdentityErrors.InvalidVerification);

        if (verification.ExpirationTime <= DateTime.UtcNow)
            return Result.Failure(IdentityErrors.InvalidVerification);

        user.EmailConfirmed = true;
        var updateResult = await userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
            return Result.Failure(IdentityErrors.EmailConfirmationFailed);

        await RemoveCodeAsync(verification, cancellationToken);

        return Result.Success();
    }

    public async Task<Result> SendPasswordResetCodeAsync(
        Guid userId,
        string email,
        CancellationToken cancellationToken = default)
    {
        var code = GenerateOtp();

        var saveResult = await SaveCodeAsync(userId, code, cancellationToken);

        if (saveResult.IsFailure)
            return saveResult;

        var bodyResult = await LoadTemplateAsync("ResetPasswordOtp.html", cancellationToken);

        if (bodyResult.IsFailure)
            return Result.Failure(bodyResult.Error!);

        var body = bodyResult.Value
            .Replace("{{otp_code}}", code)
            .Replace("{{Logo}}", "logo");

        await mailingService.SendEmailAsync(
            email,
            "Password Reset Code",
            body,
            null,
            cancellationToken);

        return Result.Success();
    }

    public async Task<Result<string>> VerifyPasswordResetCodeAsync(
        Guid userId,
        string code,
        CancellationToken cancellationToken = default)
    {
        var verification = await GetVerificationCodeAsync(
            userId,
            code,
            cancellationToken);

        if (verification is null)
            return Result<string>.Failure(IdentityErrors.InvalidVerification);

        if (verification.ExpirationTime <= DateTime.UtcNow)
            return Result<string>.Failure(IdentityErrors.InvalidVerification);

        var user = await userManager.FindByIdAsync(userId.ToString());

        if (user is null)
            return Result<string>.Failure(IdentityErrors.UserNotFound);

        await RemoveCodeAsync(verification, cancellationToken);

        var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(resetToken));

        return Result<string>.Success(encodedToken);
    }

    private async Task<Result> SaveCodeAsync(
        Guid userId,
        string code,
        CancellationToken cancellationToken)
    {
        var repository = unitOfWork.GetRepository<EmailVerificationCode>();
        var existingCode = await repository.FirstOrDefaultAsync(
            new GetVerificationCodeByUserIdSpecification(userId));

        if (existingCode is not null)
            repository.Delete(existingCode);

        var existingUser = await userManager.FindByIdAsync(userId.ToString());

        if (existingUser is null)
            return Result.Failure(IdentityErrors.UserNotFound);

        var verificationCode = new EmailVerificationCode
        {
            ApplicationUserId = existingUser.Id,
            Code = code,
            ExpirationTime = DateTime.UtcNow.AddMinutes(OtpExpirationMinutes)
        };
        await repository.AddAsync(verificationCode, cancellationToken);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    private async Task<EmailVerificationCode?> GetVerificationCodeAsync(
        Guid userId,
        string code,
        CancellationToken cancellationToken)
    {
        var repository = unitOfWork.GetRepository<EmailVerificationCode>();
        return await repository.FirstOrDefaultAsync(
            new GetVerificationCodeByUserIdAndCodeSpecification(userId, code));
    }

    private async Task RemoveCodeAsync(
        EmailVerificationCode verificationCode,
        CancellationToken cancellationToken)
    {
        var repository = unitOfWork.GetRepository<EmailVerificationCode>();
        repository.Delete(verificationCode);
        await unitOfWork.SaveChangesAsync();
    }

    private async Task<Result<string>> LoadTemplateAsync(
        string fileName,
        CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(
            environment.ContentRootPath,
            "wwwroot",
            "HTML",
            fileName);

        if (!File.Exists(filePath))
            return Result<string>.Failure(EmailErrors.TemplateNotFound);

        var body = await File.ReadAllTextAsync(filePath, cancellationToken);

        return Result<string>.Success(body);
    }

    private static string GenerateOtp()
    => Random.Shared
            .Next(100000, 1000000)
            .ToString();
}