using ECommerce.Domain.Common;
using ECommerce.UseCases.Abstractions;
using ECommerce.UseCases.Products.DTOs;
using MediatR;

namespace ECommerce.UseCases.Products.Queries.GetAllBrands;
public sealed class GetAllBrandsQueryHandler(
    IProductQueryService productQueryService)
    : IRequestHandler<
        GetAllBrandsQuery,
        Result<IReadOnlyList<GetAllBrandsResponse>>>
{
    public async Task<Result<IReadOnlyList<GetAllBrandsResponse>>> Handle(
        GetAllBrandsQuery request,
        CancellationToken cancellationToken)
    {
        var brands = await productQueryService
            .GetAllBrandsAsync(cancellationToken);

        return Result<IReadOnlyList<GetAllBrandsResponse>>
            .Success(brands);
    }
}
