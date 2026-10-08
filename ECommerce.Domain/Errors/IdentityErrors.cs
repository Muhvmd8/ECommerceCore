using ECommerce.Domain.Shared;

namespace ECommerce.Domain.Errors
{
    public static class IdentityErrors
    {
        public static readonly Error InvalidCredentials =
            Error.Validation(
                "Identity.InvalidCredentials",
                "Invalid email or password.");

        public static readonly Error EmailAlreadyExists =
            Error.Validation(
                "Identity.EmailAlreadyExists",
                "An account with this email already exists.");

        public static readonly Error UserNotFound =
            Error.NotFound(
                "Identity.UserNotFound",
                "The user was not found.");

        public static readonly Error EmailNotConfirmed =
            Error.Validation(
                "Identity.EmailNotConfirmed",
                "Email address has not been confirmed.");

        public static readonly Error InvalidVerification =
            Error.Validation(
                "Identity.InvalidVerification",
                "The verification code is invalid or has expired.");

        public static readonly Error EmailAlreadyConfirmed =
            Error.Validation(
                "Identity.EmailAlreadyConfirmed",
                "Email address is already confirmed.");

        public static readonly Error InvalidRefreshToken =
            Error.Validation(
                "Identity.InvalidRefreshToken",
                "The refresh token is invalid or has expired.");

        public static readonly Error UserCreationFailed =
            Error.Validation(
                "Identity.UserCreationFailed",
                "Failed to create the user.");

        public static readonly Error RoleNotFound =
            Error.NotFound(
                "Identity.RoleNotFound",
                "The specified role was not found.");

        public static readonly Error RoleAssignmentFailed =
            Error.Validation(
                "Identity.RoleAssignmentFailed",
                "Failed to assign the role to the user.");

        public static readonly Error PasswordChangeFailed =
            Error.Validation(
                "Identity.PasswordChangeFailed",
                "Failed to change the password.");

        public static readonly Error EmailConfirmationFailed =
            Error.Validation(
                "Identity.EmailConfirmationFailed",
                "Failed to confirm the email address.");

        public static Error CreateFailed(string message) =>
            Error.Validation(
                "Identity.CreateFailed",
                $"Failed to create the user: {message}");

        public static Error Validation(string code, string message) =>
            Error.Validation(
                code,
                message);
    }
}