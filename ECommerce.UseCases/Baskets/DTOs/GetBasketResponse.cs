using ECommerce.Domain.Entities.BasketModule;

namespace ECommerce.UseCases.Baskets.DTOs
{
    public record BasketItemResponse
    (
        int ProductId,
        string ProductName,
        string PictureUrl,
        decimal UnitPrice,
        int Quantity,
        decimal TotalPrice
    );
    public record GetBasketResponse
    (
        Guid BuyerId,
        IReadOnlyList<BasketItemResponse> Items,
        int TotalItems,
        decimal SubTotal
    )
    {
        public static GetBasketResponse From(Basket basket)
        {
            var items = basket.Items
                .Select(items => new BasketItemResponse
                (
                    items.ProductId,
                    items.ProductName,
                    items.PictureUrl,
                    items.UnitPrice,
                    items.Quantity,
                    items.LineTotal
                ))
                .ToList();

            return new GetBasketResponse
            (
                basket.BuyerId,
                items,
                basket.TotalItems,
                basket.SubTotal
            );
        }
    }

}