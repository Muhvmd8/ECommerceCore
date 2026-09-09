using ECommerce.Domain.Common;
using ECommerce.UseCases.Products.DTOs;

namespace ECommerce.UseCases.Products.Queries;
public sealed class GetAllProductsQuery(IProductQueryService productQueryService)
{
    public async Task<Result<IReadOnlyList<GetAllProductResponse>>> ExecuteAsync(CancellationToken ct = default)
    {
        var products = await productQueryService.GetAllProductsAsync(ct);
        return Result<IReadOnlyList<GetAllProductResponse>>.Success(products);
    }
}