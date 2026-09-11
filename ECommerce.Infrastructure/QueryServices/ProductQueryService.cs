using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Domain.Interfaces;
using ECommerce.UseCases.Abstractions;
using ECommerce.UseCases.Products.DTOs;
using Mapster;
using Microsoft.EntityFrameworkCore;
namespace ECommerce.Infrastructure.QueryServices;
public class ProductQueryService(IUnitOfWork unitOfWork) : IProductQueryService
{

    public async Task<IReadOnlyList<GetAllProductResponse>> GetAllProductsAsync(CancellationToken ct = default)
    {
        var productRepo = unitOfWork.GetRepository<Product, int>();
        var products = await productRepo.GetAllQueryable(null, ct, p => p.ProductType, p => p.ProductBrand)
                                .AsNoTracking()
                                .ProjectToType<GetAllProductResponse>()
                                .ToListAsync(ct);
        return products;
    }
    public async Task<IReadOnlyList<GetAllBrandsResponse>> GetAllBrandsAsync(CancellationToken ct = default)
    {
        var productBrandRepo = unitOfWork.GetRepository<ProductBrand, int>();
        var brands = await productBrandRepo.GetAllQueryable(null, ct)
                                .AsNoTracking()
                                .ProjectToType<GetAllBrandsResponse>()
                                .ToListAsync(ct);
        return brands;
    }
    public async Task<IReadOnlyList<GetAllTypesResponse>> GetAllTypesAsync(CancellationToken ct = default)
    {
        var productTypeRepo = unitOfWork.GetRepository<ProductType, int>();
        var types = await productTypeRepo.GetAllQueryable(null, ct)
                                .AsNoTracking()
                                .ProjectToType<GetAllTypesResponse>()
                                .ToListAsync(ct);
        return types;
    }
    public async Task<GetProductByIdResponse?> GetProductByIdAsync(int id, CancellationToken ct = default)
    {
        var productRepo = unitOfWork.GetRepository<Product, int>();
        var product = productRepo.GetAllQueryable(p => p.Id == id, ct, p => p.ProductType, p => p.ProductBrand)
                                .ProjectToType<GetProductByIdResponse>();

        return await product.FirstOrDefaultAsync(ct);
    }
}