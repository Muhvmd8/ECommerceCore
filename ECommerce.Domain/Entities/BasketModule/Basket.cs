using ECommerce.Domain.Errors;
using ECommerce.Domain.Shared;
using System.Text.Json.Serialization;

namespace ECommerce.Domain.Entities.BasketModule
{
    public class Basket
    {
        public Guid BuyerId { get; private set; }
        public List<BasketItem> Items { get; private set; } = [];

        [JsonConstructor]
        private Basket(Guid buyerId, List<BasketItem> items)
        {
            BuyerId = buyerId;
            Items = items;
        }

        private Basket(Guid buyerId)
        {
            BuyerId = buyerId;
            Items = [];
        }

        public static Result<Basket> CreateEmpty(Guid buyerId)
        {
            if (buyerId == Guid.Empty)
                return Result<Basket>.Failure(BasketErrors.InvalidBuyerId);

            return new Basket(buyerId);
        }

        public int TotalItems => Items.Sum(item => item.Quantity);
        public decimal SubTotal => Items.Sum(item => item.LineTotal);

        public Result AddItem(int productId, string productName, string pictureUrl,
            decimal unitPrice, int quantity)
        {
            var existingItem = Items.FirstOrDefault(item => item.ProductId == productId);

            if (existingItem is not null)
                return existingItem.IncreaseQuantity(quantity);

            var createResult = BasketItem.Create(productId, productName, pictureUrl, unitPrice, quantity);
            if (createResult.IsFailure)
                return Result.Failure(createResult.Error!);

            Items.Add(createResult.Value);
            return Result.Success();
        }

        public Result RemoveItem(int productId)
        {
            var existingItem = Items.FirstOrDefault(item => item.ProductId == productId);
            if (existingItem is null)
                return Result.Failure(BasketErrors.ItemNotFound);

            Items.Remove(existingItem);
            return Result.Success();
        }

        public Result UpdateItemQuantity(int productId, int quantity)
        {
            var existingItem = Items.FirstOrDefault(item => item.ProductId == productId);
            if (existingItem is null)
                return Result.Failure(BasketErrors.ItemNotFound);

            return existingItem.IncreaseQuantity(quantity);
        }

        public void Clear() => Items.Clear();

        public Result MergeFrom(Basket other)
        {
            if (other.BuyerId == BuyerId)
                return Result.Failure(BasketErrors.CanNotMergeSameBuyer);

            foreach (var otherItem in other.Items)
            {
                var existingItem = Items.FirstOrDefault(
                    item => item.ProductId == otherItem.ProductId);

                if (existingItem is not null)
                {
                    var increaseResult = existingItem.IncreaseQuantity(otherItem.Quantity);

                    if (increaseResult.IsFailure)
                        return Result.Failure(increaseResult.Error!);

                    continue;
                }

                Items.Add(otherItem);
            }

            return Result.Success();
        }
    }
}
