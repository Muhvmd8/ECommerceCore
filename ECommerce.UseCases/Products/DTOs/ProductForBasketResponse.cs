namespace ECommerce.UseCases.Products.DTOs
{
    public sealed record ProductForBasketResponse(
        int Id, string Name, string Description, decimal Price, string PictureUrl);
}
