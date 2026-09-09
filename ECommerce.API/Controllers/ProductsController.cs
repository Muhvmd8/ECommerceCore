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
    public async Task<ActionResult<IReadOnlyList<GetAllProductResponse>>> GetAllProducts(CancellationToken ct = default)
    {
        var products = await allProductsQuery.ExecuteAsync(ct);
        return Ok(products.Value);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<GetProductByIdResponse>> GetProductById(int id, CancellationToken ct = default)
    {
        var result = await getProductByIdQuery.ExecuteAsync(id, ct);

        return result.Match<ActionResult<GetProductByIdResponse>>(
            value => Ok(value),
            error => NotFound(error)
        );
    }
    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<GetAllBrandsResponse>>> GetAllBrands(CancellationToken ct = default)
    {
        var productBrands = await getAllBrandsQuery.ExecuteAsync(ct);
        return Ok(productBrands.Value);
    }
    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<GetAllTypesResponse>>> GetAllTypes(CancellationToken ct = default)
    {
        var productTypes = await getAllTypesQuery.ExecuteAsync(ct);
        return Ok(productTypes.Value);
    }
}