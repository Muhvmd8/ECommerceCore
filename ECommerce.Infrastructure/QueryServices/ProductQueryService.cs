using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Abstractions;
using ECommerce.UseCases.Products.DTOs;
using ECommerce.UseCases.Specifications.Specifications.Products;
namespace ECommerce.Infrastructure.QueryServices;
public class ProductQueryService(IReadRepository<Product> productRepository)
    : IProductQueryService
{

    public async Task<IReadOnlyList<GetAllProductsResponse>> GetAllProductsAsync(CancellationToken ct = default)
    {
        var products = await productRepository.ListAsync(new GetAllProductsSpecification(), ct);
        return products;
    }

    public async Task<GetProductByIdResponse?> GetProductByIdAsync(int id, CancellationToken ct = default)
    {
        var product = await productRepository.ListAsync(new GetProductByIdSpecification(id), ct);
        return product.FirstOrDefault();
    }
}