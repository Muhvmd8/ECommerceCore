using ECommerce.Domain.Common;
using ECommerce.UseCases.Products.DTOs;

namespace ECommerce.UseCases.Products.Queries;
public sealed class GetAllBrandsQuery(IProductQueryService productQueryService)
{
    public async Task<Result<IReadOnlyList<GetAllBrandsResponse>>> ExecuteAsync(CancellationToken ct = default)
    {
        var brands = await productQueryService.GetAllBrandsAsync(ct);
        return Result<IReadOnlyList<GetAllBrandsResponse>>.Success(brands);
    }
}
