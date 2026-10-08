using ECommerce.Domain.Shared;

namespace ECommerce.Domain.Errors
{
    public static class EmailErrors
    {
        public static readonly Error TemplateNotFound =
            Error.Validation(
                "Email.TemplateNotFound",
                "The email template could not be found.");

        public static readonly Error SendingFailed =
            Error.Validation(
                "Email.SendingFailed",
                "Failed to send the email.");
    }
}
