using ECommerce.Domain.Common;
using ECommerce.UseCases.Abstractions;
using ECommerce.UseCases.Products.DTOs;
using MediatR;
namespace ECommerce.UseCases.Products.Queries.GetAllProducts;
public sealed class GetAllProductsQueryHandler(IProductQueryService productQueryService)
    : IRequestHandler<
        GetAllProductsQuery,
        Result<IReadOnlyList<GetAllProductResponse>>>
{
    public async Task<Result<IReadOnlyList<GetAllProductResponse>>> Handle(GetAllProductsQuery request,
        CancellationToken cancellationToken)
    {
        var products = await productQueryService.GetAllProductsAsync(cancellationToken);
        return Result<IReadOnlyList<GetAllProductResponse>>.Success(products);
    }
}
