namespace ECommerce.Infrastructure.Persistence.Seeding.Models
{
    public record ProductSeederModel(
        string Name,
        string Description,
        decimal Price,
        string PictureUrl,
        int ProductBrandId,
        int ProductTypeId);
}
