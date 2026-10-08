using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.Products.DTOs;
using ECommerce.UseCases.Specifications.Specifications.Types;
using MediatR;
namespace ECommerce.UseCases.Features.Products.Queries.GetAllTypes;
public class GetAllTypesQueryHandler(IRepository<ProductType> repository) :
    IRequestHandler<GetAllTypesQuery, Result<IReadOnlyList<GetAllTypesResponse>>>
{
    public async Task<Result<IReadOnlyList<GetAllTypesResponse>>> Handle(GetAllTypesQuery request, CancellationToken ct)
    {
        var types = await repository.ListAsync(new GetAllProductTypeSpecification(), ct);
        return Result<IReadOnlyList<GetAllTypesResponse>>.Success(types);
    }
}