using ECommerce.Domain.Entities.ProductModule;
using ECommerce.UseCases.Features.Products.DTOs;

namespace ECommerce.UseCases.Specifications.Specifications.Products
{
    public class GetProductByIdSpecification :
        Specification<Product, GetProductByIdResponse>
    {
        public GetProductByIdSpecification(int id)
        {
            Query.Where(p => p.Id == id)
                 .Select(p => new GetProductByIdResponse
                 {
                     Id = p.Id,
                     Name = p.Name,
                     Description = p.Description,
                     Price = p.Price,
                     BrandName = p.ProductBrand.Name,
                     TypeName = p.ProductType.Name
                 });
        }
    }
}
