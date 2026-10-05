using ECommerce.Domain.Shared;

namespace ECommerce.Domain.Errors
{
    public static class UserAddressErrors
    {
        public static readonly Error InvalidAddressId =
            Error.Validation(
                "UserAddress.InvalidAddressId",
                "User address ID is required.");

        public static readonly Error InvalidUserId =
            Error.Validation(
                "UserAddress.InvalidUserId",
                "Application user ID is required.");

        public static readonly Error LabelRequired =
            Error.Validation(
                "UserAddress.LabelRequired",
                "Address label is required.");

        public static readonly Error LabelTooLong =
            Error.Validation(
                "UserAddress.LabelTooLong",
                "Address label cannot exceed 50 characters.");

        public static readonly Error CountryRequired =
            Error.Validation(
                "UserAddress.CountryRequired",
                "Country is required.");

        public static readonly Error CountryTooLong =
            Error.Validation(
                "UserAddress.CountryTooLong",
                "Country cannot exceed 100 characters.");

        public static readonly Error CityRequired =
            Error.Validation(
                "UserAddress.CityRequired",
                "City is required.");

        public static readonly Error CityTooLong =
            Error.Validation(
                "UserAddress.CityTooLong",
                "City cannot exceed 100 characters.");

        public static readonly Error StreetRequired =
            Error.Validation(
                "UserAddress.StreetRequired",
                "Street is required.");

        public static readonly Error StreetTooLong =
            Error.Validation(
                "UserAddress.StreetTooLong",
                "Street cannot exceed 250 characters.");

        public static readonly Error FirstNameRequired =
            Error.Validation(
                "UserAddress.FirstNameRequired",
                "Recipient first name is required.");

        public static readonly Error FirstNameTooLong =
            Error.Validation(
                "UserAddress.FirstNameTooLong",
                "Recipient first name cannot exceed 100 characters.");

        public static readonly Error LastNameRequired =
            Error.Validation(
                "UserAddress.LastNameRequired",
                "Recipient last name is required.");

        public static readonly Error LastNameTooLong =
            Error.Validation(
                "UserAddress.LastNameTooLong",
                "Recipient last name cannot exceed 100 characters.");

        public static readonly Error PhoneRequired =
            Error.Validation(
                "UserAddress.PhoneRequired",
                "Recipient phone number is required.");

        public static readonly Error PhoneTooLong =
            Error.Validation(
                "UserAddress.PhoneTooLong",
                "Recipient phone number cannot exceed 20 characters.");

        public static readonly Error UserAddressNotFound =
            Error.NotFound(
                "UserAddress.NotFound",
                "The user address was not found.");
    }
}