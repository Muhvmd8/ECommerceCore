using ECommerce.Domain.Common;
using ECommerce.UseCases.Abstractions;
using ECommerce.UseCases.Products.DTOs;
using MediatR;
namespace ECommerce.UseCases.Products.Queries.GetAllTypes;
public class GetAllTypesQueryHandler(IProductQueryService productQueryService) :
    IRequestHandler<GetAllTypesQuery, Result<IReadOnlyList<GetAllTypesResponse>>>
{
    public async Task<Result<IReadOnlyList<GetAllTypesResponse>>> Handle(GetAllTypesQuery request, CancellationToken ct)
    {
        var types = await productQueryService.GetAllTypesAsync(ct);
        return Result<IReadOnlyList<GetAllTypesResponse>>.Success(types);
    }
}