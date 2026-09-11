using ECommerce.UseCases.Abstractions;

namespace ECommerce.API.Endpoints;
public static class ProductsEndpoints
{
    public static IEndpointRouteBuilder MapProductsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/products").WithTags("Products");

        group.MapGet("/", async (IProductQueryService productQueryService, CancellationToken ct) =>
        {
            var products = await productQueryService.GetAllProductsAsync(ct);
            return Results.Ok(products);
        });

        return endpoints;
    }
}
