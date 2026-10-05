using ECommerce.Domain.Entities.ProductModule;
using ECommerce.UseCases.Products.DTOs;
namespace ECommerce.UseCases.Specifications.Specifications.Products;
public sealed class GetAllProductsSpecification
    : Specification<Product, GetAllProductsResponse>
{
    public GetAllProductsSpecification()
    {
        Query
            .AsNoTracking()
            .OrderBy(p => p.Price);

        Query
            .Select(p => new GetAllProductsResponse
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