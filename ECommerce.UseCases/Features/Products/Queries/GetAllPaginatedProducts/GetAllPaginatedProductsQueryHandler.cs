using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.Products.DTOs;
using ECommerce.UseCases.Specifications.Specifications.Products;
using MediatR;
namespace ECommerce.UseCases.Features.Products.Queries.GetAllPaginatedProducts;
public sealed class GetAllPaginatedProductsQueryHandler(IRepository<Product> repository)
    : IRequestHandler<GetAllPaginatedProductsQuery, Result<PagedResult<GetAllProductsResponse>>>
{
    public async Task<Result<PagedResult<GetAllProductsResponse>>> Handle(GetAllPaginatedProductsQuery request, CancellationToken cancellationToken)
    {
        var paginatedSpecifications = new GetAllPaginatedProductsSpecification(
            request.Search,
            request.ProductBrandId,
            request.ProductTypeId,
            request.SortBy,
            request.SortDescending,
            request.PageNumber,
            request.PageSize
        );
        var countSpecifications = new GetAllPaginatedProductsSpecification(
            request.Search,
            request.ProductBrandId,
            request.ProductTypeId
        );

        int totalCount = await repository.CountAsync(countSpecifications, cancellationToken);
        var products = await repository.ListAsync(paginatedSpecifications, cancellationToken);

        var paginatedResult = new PagedResult<GetAllProductsResponse>(products, totalCount);

        var pagedProducts = Result<PagedResult<GetAllProductsResponse>>.Success(paginatedResult);
        return pagedProducts;
    }
}