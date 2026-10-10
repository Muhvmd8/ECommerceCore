using ECommerce.Domain.Constants;
using ECommerce.Domain.Errors;
using ECommerce.Domain.Shared;
using System.Text.Json.Serialization;

namespace ECommerce.Domain.Entities.BasketModule
{
    public class BasketItem
    {
        // System.Text.Json
        [JsonConstructor]
        private BasketItem(int productId, string productName, string pictureUrl, decimal unitPrice, int quantity)
        {
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }

        public int ProductId { get; private set; }
        public string ProductName { get; private set; } = string.Empty;
        public string PictureUrl { get; private set; } = string.Empty;
        public decimal UnitPrice { get; private set; }
        public int Quantity { get; private set; }
        public decimal LineTotal => UnitPrice * Quantity;
        public static Result<BasketItem> Create(
            int productId,
            string productName,
            string pictureUrl,
            decimal unitPrice,
            int quantity)
        {
            var productIdResult = SetProductId(productId);
            if (productIdResult.IsFailure)
                return Result<BasketItem>.Failure(productIdResult.Error!);

            var productNameResult = SetProductName(productName);
            if (productNameResult.IsFailure)
                return Result<BasketItem>.Failure(productNameResult.Error!);

            var pictureUrlResult = SetPictureUrl(pictureUrl);
            if (pictureUrlResult.IsFailure)
                return Result<BasketItem>.Failure(pictureUrlResult.Error!);

            var unitPriceResult = SetUnitPrice(unitPrice);
            if (unitPriceResult.IsFailure)
                return Result<BasketItem>.Failure(unitPriceResult.Error!);

            var quantityResult = SetQuantity(quantity);
            if (quantityResult.IsFailure)
                return Result<BasketItem>.Failure(quantityResult.Error!);

            return Result<BasketItem>.Success(
                new BasketItem(
                    productId,
                    productName.Trim(),
                    pictureUrl.Trim(),
                    unitPrice,
                    quantity));
        }

        private static Result SetProductId(int productId)
            => productId > 0
                ? Result.Success()
                : Result.Failure(BasketErrors.InvalidProductId);

        private static Result SetProductName(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
                return Result.Failure(BasketErrors.ProductNameRequired);

            return Result.Success();
        }

        private static Result SetPictureUrl(string pictureUrl)
        {
            if (string.IsNullOrWhiteSpace(pictureUrl))
                return Result.Failure(BasketErrors.PictureUrlRequired);

            return Result.Success();
        }

        private static Result SetUnitPrice(decimal unitPrice)
            => unitPrice > 0
                ? Result.Success()
                : Result.Failure(BasketErrors.InvalidUnitPrice);

        private static Result SetQuantity(int quantity)
            => quantity >= BasketItemConstants.MinQuantity &&
               quantity <= BasketItemConstants.MaxQuantity
                ? Result.Success()
                : Result.Failure(BasketErrors.InvalidQuantity);

        public Result IncreaseQuantity(int amount)
        {
            if (amount <= 0)
                return Result.Failure(BasketErrors.InvalidAmountIncrement);

            var newQuantity = Quantity + amount;

            if (Quantity >= BasketItemConstants.MaxQuantity)
                return Result.Failure(BasketErrors.QuantityTooHigh);

            Quantity = newQuantity;

            return Result.Success();
        }
        public Result SetSpecifiedQuantity(int quantity)
        {
            if (quantity is < BasketItemConstants.MinQuantity or > BasketItemConstants.MaxQuantity)
                return Result.Failure(BasketErrors.InvalidQuantity);

            Quantity = quantity;
            return Result.Success();
        }
        public Result UpdateUnitPrice(decimal unitPrice)
        {
            if (unitPrice <= 0)
                return Result.Failure(BasketErrors.InvalidUnitPrice);

            UnitPrice = unitPrice;
            return Result.Success();
        }
    }
}