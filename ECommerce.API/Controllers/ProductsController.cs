using ECommerce.API.Models;
using ECommerce.UseCases.Products.DTOs;
using ECommerce.UseCases.Products.Queries;
using Microsoft.AspNetCore.Mvc;
namespace ECommerce.API.Controllers;
public class ProductsController(
    GetAllProductsQuery allProductsQuery,
    GetAllTypesQuery getAllTypesQuery,
    GetAllBrandsQuery getAllBrandsQuery,
    GetProductByIdQuery getProductByIdQuery
    ) : ApiController
{

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<GetAllProductResponse>>>> GetAllProducts(CancellationToken ct = default)
    {
        var result = await allProductsQuery.ExecuteAsync(ct);
        return FromResult(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<GetProductByIdResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<GetProductByIdResponse>>> GetProductById(int id, CancellationToken ct = default)
    {
        var result = await getProductByIdQuery.ExecuteAsync(id, ct);
        return FromResult(result);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<GetAllBrandsResponse>>>> GetAllBrands(CancellationToken ct = default)
    {
        var result = await getAllBrandsQuery.ExecuteAsync(ct);
        return FromResult(result);
    }

    [HttpGet("types")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<GetAllTypesResponse>>>> GetAllTypes(CancellationToken ct = default)
    {
        var result = await getAllTypesQuery.ExecuteAsync(ct);
        return FromResult(result);
    }
}