using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.Infrastructure.Persistence.Interceptors;
using ECommerce.Infrastructure.Persistence.Seeding;
using ECommerce.Infrastructure.Persistence.Seeding.Abstraction;
using ECommerce.Infrastructure.Persistence.Seeding.Implementation;
using ECommerce.Infrastructure.QueryServices;
using ECommerce.Infrastructure.Repositories;
using ECommerce.UseCases.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace ECommerce.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString)
                .EnableSensitiveDataLogging();
        });

        services.AddScoped<IDataSeeder, BrandSeeder>();
        services.AddScoped<IDataSeeder, TypeSeeder>();
        services.AddScoped<DatabaseSeeder>();

        services.AddScoped<IAuditInterceptor, AuditInterceptor>();
        services.AddScoped<ISoftDeleteInterceptor, SoftDeleteInterceptor>();

        services.AddScoped<IProductQueryService, ProductQueryService>();

        return services;
    }
}