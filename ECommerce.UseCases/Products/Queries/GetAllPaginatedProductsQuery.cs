using ECommerce.Domain.Common;
using ECommerce.UseCases.Products.DTOs;
using ECommerce.UseCases.Products.Enums;
using MediatR;
namespace ECommerce.UseCases.Products.Queries;
public sealed record GetAllPaginatedProductsQuery
    : IRequest<Result<PagedResult<GetAllProductsResponse>>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? Search { get; set; }
    public int? ProductBrandId { get; set; }
    public int? ProductTypeId { get; set; }
    public ProductsSortField? SortBy { get; set; } = ProductsSortField.Name;
    public bool SortDescending { get; set; } = false;
}