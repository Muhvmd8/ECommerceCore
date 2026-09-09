using ECommerce.UseCases.Products.Queries;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.UseCases;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(DependencyInjection).Assembly);
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        services.AddScoped<GetAllProductsQuery>();
        services.AddScoped<GetProductByIdQuery>();
        services.AddScoped<GetAllBrandsQuery>();
        services.AddScoped<GetAllTypesQuery>();

        return services;
    }
}