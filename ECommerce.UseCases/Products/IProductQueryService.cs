using ECommerce.UseCases.Products.DTOs;
namespace ECommerce.UseCases.Products;
public interface IProductQueryService
{
    Task<IReadOnlyList<GetAllProductResponse>> GetAllProductsAsync(CancellationToken ct = default);
    Task<IReadOnlyList<GetAllTypesResponse>> GetAllTypesAsync(CancellationToken ct = default);
    Task<IReadOnlyList<GetAllBrandsResponse>> GetAllBrandsAsync(CancellationToken ct = default);
    Task<GetProductByIdResponse?> GetProductByIdAsync(int id, CancellationToken ct = default);
}