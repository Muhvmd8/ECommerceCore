using ECommerce.Domain.Common;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Domain.Interfaces;
using ECommerce.UseCases.Products.DTOs;
using ECommerce.UseCases.Specifications.Specifications.Types;
using MediatR;
namespace ECommerce.UseCases.Products.Queries.Handlers;
public class GetAllTypesQueryHandler(IRepository<ProductType> repository) :
    IRequestHandler<GetAllTypesQuery, Result<IReadOnlyList<GetAllTypesResponse>>>
{
    public async Task<Result<IReadOnlyList<GetAllTypesResponse>>> Handle(GetAllTypesQuery request, CancellationToken ct)
    {
        var types = await repository.ListAsync(new GetAllProductTypeSpecification(), ct);
        return Result<IReadOnlyList<GetAllTypesResponse>>.Success(types);
    }
}