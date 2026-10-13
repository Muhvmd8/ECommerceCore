using CloudinaryDotNet;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Settings;
using ECommerce.Infrastructure.Caching;
using ECommerce.Infrastructure.Email;
using ECommerce.Infrastructure.Identity;
using ECommerce.Infrastructure.Identity.Services;
using ECommerce.Infrastructure.Notifications;
using ECommerce.Infrastructure.Payments;
using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.Infrastructure.Persistence.Interceptors;
using ECommerce.Infrastructure.Persistence.Seeding;
using ECommerce.Infrastructure.Persistence.Seeding.Abstraction;
using ECommerce.Infrastructure.Persistence.Seeding.Implementation;
using ECommerce.Infrastructure.QueryServices;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Security;
using ECommerce.Infrastructure.Services;
using ECommerce.UseCases.Abstractions;
using ECommerce.UseCases.Commons.Options;
using ECommerce.UseCases.Commons.Options.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
namespace ECommerce.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services._AddRepositoriesServices();
        services._AddEfCoreServices(configuration);
        services.AddScoped<IProductQueryService, ProductQueryService>();
        services._AddSeedingServices();
        services._AddCachingServices(configuration);
        services._ConfigureInfrastructureSettings(configuration);
        services._AddInfrastructureCommonServices();
        services._AddCloudinaryService(configuration);

        return services;
    }
    private static IServiceCollection _AddEfCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString,
                sql => sql.MigrationsHistoryTable("__ApplicationMigrationHistory"))
                .EnableSensitiveDataLogging();
        });
        services.AddDbContext<ApplicationIdentityDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString,
                sql => sql.MigrationsHistoryTable("__IdentityMigrationHistory"))
                .EnableSensitiveDataLogging();
        });
        services.AddScoped<IAuditInterceptor, AuditInterceptor>();
        services.AddScoped<ISoftDeleteInterceptor, SoftDeleteInterceptor>();

        return services;
    }
    private static IServiceCollection _AddCloudinaryService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(sp =>
        {
            var s = sp.GetRequiredService<IOptions<CloudinarySettings>>().Value;
            var cloudinary = new Cloudinary(new Account(s.CloudName, s.ApiKey, s.ApiSecret));
            cloudinary.Api.Secure = true;
            return cloudinary;
        });

        return services;
    }
    private static IServiceCollection _AddRepositoriesServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IReadRepository<>), typeof(Repository<>));
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IEmailStore, EmailStore>();

        return services;
    }
    private static IServiceCollection _ConfigureInfrastructureSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SuperAdminSettings>(configuration.GetSection($"Seed:{SuperAdminSettings.SectionName}"));
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.Configure<SmtpSettings>(configuration.GetSection(SmtpSettings.SectionName));
        services.AddSingleton<IValidateOptions<JwtSettings>, JwtSettingsValidator>();
        services.Configure<StripeSettings>(configuration.GetSection(StripeSettings.SectionName));
        services.Configure<CloudinarySettings>(configuration.GetSection(CloudinarySettings.SectionName));
        services.Configure<OtpSettings>(configuration.GetSection(OtpSettings.SectionName));

        return services;
    }
    private static IServiceCollection _AddSeedingServices(this IServiceCollection services)
    {
        services.AddScoped<IDataSeeder, BrandSeeder>();
        services.AddScoped<IDataSeeder, TypeSeeder>();
        services.AddScoped<IDataSeeder, ProductSeeder>();
        services.AddScoped<IDataSeeder, IdentitySeeder>();
        services.AddScoped<IDataSeeder, DeliveryMethodSeeder>();
        services.AddScoped<DatabaseSeeder>();

        return services;
    }
    private static IServiceCollection _AddInfrastructureCommonServices(this IServiceCollection services)
    {
        services.AddScoped<IIpSecurityService, IpSecurityService>();
        services.AddScoped<IMailingService, MailingService>();
        services.AddScoped<IEmailVerificationService, EmailVerificationService>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IPaymentService, StripePaymentService>();
        services.AddScoped<IAttachmentService, AttachmentService>();
        services.AddScoped<INotificationCommandService, NotificationCommandService>();
        services.AddScoped<INotificationQueryService, NotificationQueryService>();

        return services;
    }
    private static IServiceCollection _AddCachingServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<CacheEntryPolicy>("Basket")
            .BindConfiguration("CachedAggregate:Basket")
            .ValidateOnStart();

        services
            .AddOptions<CacheEntryPolicy>("EmailVerificationCode")
            .BindConfiguration("CachedAggregate:EmailVerificationCode")
            .ValidateOnStart();

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