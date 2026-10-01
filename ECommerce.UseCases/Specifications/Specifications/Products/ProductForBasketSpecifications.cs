using ECommerce.Domain.Entities.ProductModule;
using ECommerce.UseCases.Products.DTOs;

namespace ECommerce.UseCases.Specifications.Specifications.Products
{
    public sealed class ProductForBasketSpecification
        : Specification<Product, ProductForBasketResponse>
    {
        public ProductForBasketSpecification(int id)
        {
            Query
                .Where(p => p.Id == id)
                .Select(p => new ProductForBasketResponse(
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Price,
                    p.PictureUrl
                ));
        }
    }
}
