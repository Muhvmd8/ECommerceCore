using ECommerce.Domain.Entities.Common;

namespace ECommerce.Domain.Entities.ProductModule
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }
        public string PictureUrl { get; set; } = default!;
        public int ProductBrandId { get; set; }
        public ProductBrand ProductBrand { get; set; } = default!;
        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; } = default!;
    }
}
