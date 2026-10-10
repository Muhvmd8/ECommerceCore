using ECommerce.Domain.Constants;
using ECommerce.Domain.Entities.Common;
using ECommerce.Domain.Errors;
using ECommerce.Domain.Shared;

namespace ECommerce.Domain.Entities.UserModule
{
    public class UserAddress : BaseEntity<Guid>
    {
        #region Properties
        public Guid ApplicationUserId { get; private set; } = default!;
        public string Label { get; private set; } = default!;
        public string Country { get; private set; } = default!;
        public string City { get; private set; } = default!;
        public string Street { get; private set; } = default!;
        public string RecipientFirstName { get; private set; } = default!;
        public string RecipientLastName { get; private set; } = default!;
        public string Phone { get; private set; } = default!;
        public bool IsDefault { get; private set; }
        #endregion

        #region Constructor
        private UserAddress(
            Guid Id,
            Guid applicationUserId,
            string label,
            string country,
            string city,
            string street,
            string recipientFirstName,
            string recipientLastName,
            string phone,
            bool isDefault)
        {
            ApplicationUserId = applicationUserId;
            Label = label;
            Country = country;
            City = city;
            Street = street;
            RecipientFirstName = recipientFirstName;
            RecipientLastName = recipientLastName;
            Phone = phone;
            IsDefault = isDefault;
        }

        #endregion

        #region Factory Method
        public static Result<UserAddress> Create(
            Guid id,
            Guid applicationUserId,
            string label,
            string country,
            string city,
            string street,
            string recipientFirstName,
            string recipientLastName,
            string phone,
            bool isDefault = false)
        {
            var idResult = ValidateAddressId(id);
            if (idResult.IsFailure)
                return Result<UserAddress>.Failure(idResult.Error!);

            var applicationUserIdResult = ValidateApplicationUserId(applicationUserId);
            if (applicationUserIdResult.IsFailure)
                return Result<UserAddress>.Failure(applicationUserIdResult.Error!);

            var labelResult = ValidateLabel(label);
            if (labelResult.IsFailure)
                return Result<UserAddress>.Failure(labelResult.Error!);

            var countryResult = ValidateCountry(country);
            if (countryResult.IsFailure)
                return Result<UserAddress>.Failure(countryResult.Error!);

            var cityResult = ValidateCity(city);
            if (cityResult.IsFailure)
                return Result<UserAddress>.Failure(cityResult.Error!);

            var streetResult = ValidateStreet(street);
            if (streetResult.IsFailure)
                return Result<UserAddress>.Failure(streetResult.Error!);

            var firstNameResult = ValidateFirstName(recipientFirstName);
            if (firstNameResult.IsFailure)
                return Result<UserAddress>.Failure(firstNameResult.Error!);

            var lastNameResult = ValidateLastName(recipientLastName);
            if (lastNameResult.IsFailure)
                return Result<UserAddress>.Failure(lastNameResult.Error!);

            var phoneResult = ValidatePhone(phone);
            if (phoneResult.IsFailure)
                return Result<UserAddress>.Failure(phoneResult.Error!);

            return Result<UserAddress>.Success(
                new UserAddress(
                    id,
                    applicationUserId,
                    label.Trim(),
                    country.Trim(),
                    city.Trim(),
                    street.Trim(),
                    recipientFirstName.Trim(),
                    recipientLastName.Trim(),
                    phone.Trim(),
                    isDefault));
        }

        private static Result ValidateAddressId(Guid id)
        {
            if (id == Guid.Empty)
                return Result.Failure(UserAddressErrors.InvalidAddressId);
            return Result.Success();
        }

        #endregion

        #region Update
        public Result Update(
            string label,
            string country,
            string city,
            string street,
            string recipientFirstName,
            string recipientLastName,
            string phone)
        {
            var labelResult = ValidateLabel(label);
            if (labelResult.IsFailure)
                return labelResult;

            var countryResult = ValidateCountry(country);
            if (countryResult.IsFailure)
                return countryResult;

            var cityResult = ValidateCity(city);
            if (cityResult.IsFailure)
                return cityResult;

            var streetResult = ValidateStreet(street);
            if (streetResult.IsFailure)
                return streetResult;

            var firstNameResult = ValidateFirstName(recipientFirstName);
            if (firstNameResult.IsFailure)
                return firstNameResult;

            var lastNameResult = ValidateLastName(recipientLastName);
            if (lastNameResult.IsFailure)
                return lastNameResult;

            var phoneResult = ValidatePhone(phone);
            if (phoneResult.IsFailure)
                return phoneResult;

            Label = label.Trim();
            Country = country.Trim();
            City = city.Trim();
            Street = street.Trim();
            RecipientFirstName = recipientFirstName.Trim();
            RecipientLastName = recipientLastName.Trim();
            Phone = phone.Trim();

            return Result.Success();
        }

        public void SetAsDefault()
            => IsDefault = true;

        public void RemoveAsDefault()
            => IsDefault = false;

        #endregion

        #region Validation
        private static Result ValidateApplicationUserId(Guid applicationUserId)
        {
            if (applicationUserId == Guid.Empty)
                return Result.Failure(UserAddressErrors.InvalidUserId);
            return Result.Success();
        }

        private static Result ValidateLabel(string label)
        {
            if (string.IsNullOrWhiteSpace(label))
                return Result.Failure(UserAddressErrors.LabelRequired);

            if (label.Length > UserAddressConstants.MaxLabelLength)
                return Result.Failure(UserAddressErrors.LabelTooLong);

            return Result.Success();
        }

        private static Result ValidateCountry(string country)
        {
            if (string.IsNullOrWhiteSpace(country))
                return Result.Failure(UserAddressErrors.CountryRequired);

            if (country.Length > UserAddressConstants.MaxCountryLength)
                return Result.Failure(UserAddressErrors.CountryTooLong);

            return Result.Success();
        }

        private static Result ValidateCity(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return Result.Failure(UserAddressErrors.CityRequired);

            if (city.Length > UserAddressConstants.MaxCityLength)
                return Result.Failure(UserAddressErrors.CityTooLong);

            return Result.Success();
        }

        private static Result ValidateStreet(string street)
        {
            if (string.IsNullOrWhiteSpace(street))
                return Result.Failure(UserAddressErrors.StreetRequired);

            if (street.Length > UserAddressConstants.MaxStreetLength)
                return Result.Failure(UserAddressErrors.StreetTooLong);

            return Result.Success();
        }

        private static Result ValidateFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                return Result.Failure(UserAddressErrors.FirstNameRequired);

            if (firstName.Length > UserAddressConstants.MaxFirstNameLength)
                return Result.Failure(UserAddressErrors.FirstNameTooLong);

            return Result.Success();
        }

        private static Result ValidateLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
                return Result.Failure(UserAddressErrors.LastNameRequired);

            if (lastName.Length > UserAddressConstants.MaxLastNameLength)
                return Result.Failure(UserAddressErrors.LastNameTooLong);

            return Result.Success();
        }

        private static Result ValidatePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return Result.Failure(UserAddressErrors.PhoneRequired);

            if (phone.Length > UserAddressConstants.MaxPhoneLength)
                return Result.Failure(UserAddressErrors.PhoneTooLong);

            return Result.Success();
        }
        #endregion
    }
}