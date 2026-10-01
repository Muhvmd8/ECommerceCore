using ECommerce.Domain.Consts;
using ECommerce.Domain.Shared;

namespace ECommerce.Domain.Errors
{
    public static class BasketErrors
    {
        public static readonly Error BasketNotFound =
            Error.NotFound(
                "Basket.NotFound",
                "The basket was not found.");

        public static readonly Error AnonymousBasketNotFound =
            Error.NotFound(
                "Basket.NotFound",
                "The anonymous basket was not found.");

        public static readonly Error GuestBuyerIdRequired =
            Error.Validation(
                "Basket.GuestBuyerIdRequired",
                "Guest shoppers must send the X-Buyer-Id header with a client-generated buyer id.");

        public static readonly Error AuthenticatedBuyerIdMissing =
            Error.Validation(
                "Basket.AuthenticatedBuyerIdMissing",
                "The user id claim is missing or invalid in the authentication token.");

        public static readonly Error CanNotMergeSameBuyer =
            Error.Validation(
                "Basket.CanNotMergeSameBuyer",
                "Cannot merge baskets belonging to the same buyer.");

        public static readonly Error InvalidBuyerId =
            Error.Validation(
                "Basket.InvalidBuyerId",
                "A valid buyer id is required.");

        public static readonly Error ItemNotFound =
            Error.NotFound(
                "Item.NotFound",
                "The item was not found.");

        public static readonly Error InvalidProductId =
            Error.Validation(
                "Basket.InvalidProductId",
                "A valid product id is required.");

        public static readonly Error ProductNameRequired =
            Error.Validation(
                "BasketItem.ProductNameRequired",
                "Product name is required.");

        public static readonly Error PictureUrlRequired =
            Error.Validation(
                "BasketItem.PictureUrlRequired",
                "Picture URL is required.");

        public static readonly Error InvalidUnitPrice =
            Error.Validation(
                "BasketItem.InvalidUnitPrice",
                "Unit price must be greater than zero.");

        public static readonly Error InvalidQuantity =
            Error.Validation(
                "BasketItem.InvalidQuantity",
                $"Quantity must be between {BasketItemConsts.MinQuantity} and {BasketItemConsts.MaxQuantity}.");

        public static readonly Error QuantityTooHigh =
            Error.Validation(
                "BasketItem.InvalidQuantityIncrement",
                $"Quantity cannot exceed {BasketItemConsts.MaxQuantity}.");

        public static readonly Error InvalidAmountIncrement =
            Error.Validation(
                "Basket.InvalidAmountIncrement",
                "The quantity cannot be increased by the specified amount.");

    }
}
