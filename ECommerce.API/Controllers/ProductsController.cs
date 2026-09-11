using ECommerce.API.Models;
using ECommerce.UseCases.Products.DTOs;
using ECommerce.UseCases.Products.Queries.GetAllBrands;
using ECommerce.UseCases.Products.Queries.GetAllProducts;
using ECommerce.UseCases.Products.Queries.GetAllTypes;
using ECommerce.UseCases.Products.Queries.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace ECommerce.API.Controllers;
public class ProductsController(IMediator mediator) : ApiController
{

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<GetAllProductResponse>>>> GetAllProducts(CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetAllProductsQuery(), ct);
        return FromResult(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<GetProductByIdResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<GetProductByIdResponse>>> GetProductById(int id, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetProductByIdQuery(id), ct);
        return FromResult(result);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<GetAllBrandsResponse>>>> GetAllBrands(CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetAllBrandsQuery(), ct);
        return FromResult(result);
    }

    [HttpGet("types")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<GetAllTypesResponse>>>> GetAllTypes(CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetAllTypesQuery(), ct);
        return FromResult(result);
    }
}