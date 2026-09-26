using ECommerce.Domain.Entities.ProductModule;
using ECommerce.UseCases.Products.DTOs;

namespace ECommerce.UseCases.Specifications.Specifications.Types
{
    internal class GetAllProductTypeSpecification :
    Specification<ProductType, GetAllTypesResponse>
    {
        public GetAllProductTypeSpecification()
        {
            Query.Select(pb => new GetAllTypesResponse(pb.Id, pb.Name))
                 .OrderBy(pb => pb.Name)
                 .AsNoTracking();
        }
    }
}