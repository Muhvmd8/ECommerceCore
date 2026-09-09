using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.UseCases.Products.DTOs;
namespace ECommerce.UseCases.Products.Queries;
public sealed class GetProductByIdQuery(IProductQueryService productQueryService)
{
    public async Task<Result<GetProductByIdResponse>> ExecuteAsync(int id, CancellationToken ct = default)
    {
        var product = await productQueryService.GetProductByIdAsync(id, ct);

        if (product is null)
            return Result<GetProductByIdResponse>.Failure(ProductErrors.ProductNotFound);

        return Result<GetProductByIdResponse>.Success(product);
    }
}
