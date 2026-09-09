using ECommerce.Domain.Common;
using ECommerce.UseCases.Products.DTOs;

namespace ECommerce.UseCases.Products.Queries;
public sealed class GetAllTypesQuery(IProductQueryService productQueryService)
{
    public async Task<Result<IReadOnlyList<GetAllTypesResponse>>> ExecuteAsync(CancellationToken ct = default)
    {
        var types = await productQueryService.GetAllTypesAsync(ct);
        return Result<IReadOnlyList<GetAllTypesResponse>>.Success(types);
    }
}
