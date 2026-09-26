using ECommerce.Domain.Entities.ProductModule;
using ECommerce.UseCases.Products.DTOs;
using Mapster;
namespace ECommerce.UseCases.MappingConfigurations;
public class MappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        #region Product Mapping
        config.NewConfig<Product, GetAllProductsResponse>()
            .Map(dest => dest.BrandName, src => src.ProductBrand.Name)
            .Map(dest => dest.TypeName, src => src.ProductType.Name);

        config.NewConfig<Product, GetProductByIdResponse>()
            .Map(dest => dest.BrandName, src => src.ProductBrand.Name)
            .Map(dest => dest.TypeName, src => src.ProductType.Name);
        #endregion

        #region Brand & Type Mapping
        config.NewConfig<ProductBrand, GetAllBrandsResponse>();
        config.NewConfig<ProductBrand, GetAllTypesResponse>();
        #endregion
    }
}