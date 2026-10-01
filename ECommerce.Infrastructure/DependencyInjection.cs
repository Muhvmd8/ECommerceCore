using ECommerce.Domain.Interfaces.Repositories;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Caching;
using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.Infrastructure.Persistence.Interceptors;
using ECommerce.Infrastructure.Persistence.Seeding;
using ECommerce.Infrastructure.Persistence.Seeding.Abstraction;
using ECommerce.Infrastructure.Persistence.Seeding.Implementation;
using ECommerce.Infrastructure.QueryServices;
using ECommerce.Infrastructure.Repositories;
using ECommerce.UseCases.Abstractions;
using ECommerce.UseCases.Commons.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
namespace ECommerce.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IReadRepository<>), typeof(Repository<>));
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
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

        AddCachingServices(services, configuration);

        return services;
    }

    private static IServiceCollection AddCachingServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CacheEntryPolicy>(configuration.GetSection("CacheEntryPolicy"));
        services.AddSingleton<IValidateOptions<CacheEntryPolicy>, CacheEntryPolicyValidator>();

        var redisConnection = configuration.GetConnectionString("RedisConnection");

        if (!string.IsNullOrWhiteSpace(redisConnection))
            services.AddStackExchangeRedisCache(options =>
                options.Configuration = redisConnection);

        services.AddScoped(typeof(ICachedAggregateStore<>), typeof(CachedAggregateStore<>));
        services.AddScoped<IBasketRepository, BasketRepository>();

        services.AddHybridCache();

        return services;
    }
}