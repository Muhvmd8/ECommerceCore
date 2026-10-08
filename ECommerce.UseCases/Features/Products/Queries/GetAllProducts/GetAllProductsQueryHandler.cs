using ECommerce.Domain.Shared;
using ECommerce.UseCases.Abstractions;
using ECommerce.UseCases.Features.Products.DTOs;
using MediatR;
namespace ECommerce.UseCases.Features.Products.Queries.GetAllProducts;
public sealed class GetAllProductsQueryHandler(IProductQueryService productQueryService)
    : IRequestHandler<
        GetAllProductsQuery,
        Result<IReadOnlyList<GetAllProductsResponse>>>
{
    public async Task<Result<IReadOnlyList<GetAllProductsResponse>>> Handle(GetAllProductsQuery request,
        CancellationToken cancellationToken)
    {
        var products = await productQueryService.GetAllProductsAsync(cancellationToken);
        return Result<IReadOnlyList<GetAllProductsResponse>>.Success(products);
    }
}
