using ECommerce.Domain.Entities.ProductModule;
using ECommerce.UseCases.Products.DTOs;

namespace ECommerce.UseCases.Specifications.Specifications.Brands
{
    internal class GetAllProductBrandSpecification :
        Specification<ProductBrand, GetAllBrandsResponse>
    {
        public GetAllProductBrandSpecification()
        {
            Query.Select(pb => new GetAllBrandsResponse(pb.Id, pb.Name))
                 .OrderBy(pb => pb.Name)
                 .AsNoTracking();
        }
    }
}