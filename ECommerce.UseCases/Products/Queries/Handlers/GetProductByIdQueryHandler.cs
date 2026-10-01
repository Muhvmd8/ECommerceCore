using ECommerce.Domain.Errors;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Abstractions;
using ECommerce.UseCases.Products.DTOs;
using MediatR;
namespace ECommerce.UseCases.Products.Queries.Handlers;
internal class GetProductByIdQueryHandler(IProductQueryService productQueryService) :
    IRequestHandler<GetProductByIdQuery, Result<GetProductByIdResponse>>
{
    public async Task<Result<GetProductByIdResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await productQueryService.GetProductByIdAsync(request.Id, cancellationToken);

        if (product is null)
            return Result<GetProductByIdResponse>.Failure(ProductErrors.ProductNotFound);

        return Result<GetProductByIdResponse>.Success(product);
    }
}