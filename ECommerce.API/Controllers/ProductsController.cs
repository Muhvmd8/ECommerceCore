using ECommerce.API.Models;
using ECommerce.UseCases.Features.Products.DTOs;
using ECommerce.UseCases.Features.Products.Queries.GetAllPaginatedProducts;
using ECommerce.UseCases.Features.Products.Queries.GetAllProducts;
using ECommerce.UseCases.Features.Products.Queries.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace ECommerce.API.Controllers;
public class ProductsController(IMediator mediator) : ApiController
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<GetAllProductsResponse>>>> GetAllProducts(CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetAllProductsQuery(), ct);
        return FromResult(result);
    }

    [HttpGet("paged")]
    //[OutputCache(Duration = 60, Tags = ["Products"])]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<GetAllProductsResponse>>>> GetAllProducts(
        [FromQuery] GetAllPaginatedProductsQuery query,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(query, ct);
        return FromPagedResult(result, query.PageNumber, query.PageSize, "Products Retrieved Successfully");
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<GetProductByIdResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<GetProductByIdResponse>>> GetProductById(int id, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetProductByIdQuery(id), ct);
        return FromResult(result);
    }
}