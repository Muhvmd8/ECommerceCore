using ECommerce.UseCases.Products.DTOs;
namespace ECommerce.UseCases.Abstractions;
public interface IProductQueryService
{
    Task<IReadOnlyList<GetAllProductsResponse>> GetAllProductsAsync(CancellationToken ct = default);
    Task<GetProductByIdResponse?> GetProductByIdAsync(int id, CancellationToken ct = default);
}