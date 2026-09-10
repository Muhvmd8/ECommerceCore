using ECommerce.API.Middlewares;

namespace ECommerce.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(); // generate openapi file

        services.AddProblemDetails();
        services.AddScoped<GlobalExceptionMiddleware>();

        return services;
    }
}
