using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Products.DTOs;
using ECommerce.UseCases.Specifications.Specifications.Brands;
using MediatR;

namespace ECommerce.UseCases.Products.Queries.Handlers;
public sealed class GetAllBrandsQueryHandler(
    IReadRepository<ProductBrand> repository)
    : IRequestHandler<
        GetAllBrandsQuery,
        Result<IReadOnlyList<GetAllBrandsResponse>>>
{
    public async Task<Result<IReadOnlyList<GetAllBrandsResponse>>> Handle(
        GetAllBrandsQuery request,
        CancellationToken cancellationToken)
    {
        var brands = await repository.ListAsync(new GetAllProductBrandSpecification(), cancellationToken);

        return Result<IReadOnlyList<GetAllBrandsResponse>>
            .Success(brands);
    }
}