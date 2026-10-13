using Asp.Versioning;
using ECommerce.API.Endpoints;
using ECommerce.API.Filters;
using ECommerce.API.Hubs.Notifications;
using ECommerce.API.Middlewares.Extensions;
using ECommerce.Infrastructure;
using ECommerce.Infrastructure.Identity;
using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.Infrastructure.Persistence.Seeding;
using ECommerce.UseCases;
using ECommerce.UseCases.Abstractions;
using ECommerce.UseCases.Commons.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Ui.Core.Extensions;
using Serilog.Ui.MsSqlServerProvider.Extensions;
using Serilog.Ui.Web.Extensions;
using System.Text;
using System.Threading.RateLimiting;
namespace ECommerce.API;
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region Add services to the container.

        builder.Services.AddApiServices();
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddApplicationServices();

        #region Serilog Configurations
        builder.Host.UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext();
        });

        builder.Services.AddSerilogUi(options =>
            options.UseSqlServer(opts => opts
                .WithConnectionString(builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty)
                .WithSchema("Logging")
                .WithTable("Logs")
            )
            .AddScopedAsyncAuthFilter<SerilogUiAuthorizationFilter>()
        );
        #endregion

        #region OutputCache
        builder.Services.AddOutputCache(options =>
{
    options.AddPolicy("Products", policy =>
    {
        policy.Expire(TimeSpan.FromMinutes(1))
            .SetVaryByQuery("PageNumber", "PageSize");
    });
});
        #endregion

        #region Add EF Core Identity Configurations
        builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;
})
            .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
            .AddDefaultTokenProviders();
        #endregion

        #region Add Identity & Authentication Services
        builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"] ?? string.Empty)),
                    ClockSkew = TimeSpan.FromMinutes(3),
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        if (!string.IsNullOrEmpty(accessToken) &&
                            context.HttpContext.Request.Path.StartsWithSegments(
                                "/hubs/notifications"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        builder.Services.AddAuthorization();
        #endregion

        #region Settings
        builder.Services.Configure<IpSecuritySettings>(builder.Configuration.GetSection(nameof(IpSecuritySettings)));
        #endregion

        #region Forwarded Headers Configuration
        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor
                                     | ForwardedHeaders.XForwardedProto;
        });
        #endregion

        #region CORS
        var allowedOrigins = new[]
        {
            "https://localhost:4200"
        };

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("UnifiedPolicy", policy =>
            {
                policy.WithOrigins(allowedOrigins)
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });
        #endregion

        #region RateLimiter Configuration
        builder.Services.AddMemoryCache();
        builder.Services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 100,
                        Window = TimeSpan.FromSeconds(10),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 2
                    }
                )
            );
        });
        #endregion

        #region Hashing Decryption Configuration
        //((IConfigurationBuilder)builder.Configuration).Add(new DecryptedConfigurationSource());
        #endregion

        #region SignalR Configurations
        builder.Services.AddSignalR();
        builder.Services.AddSingleton<IUserIdProvider, NameIdentifierUserIdProvider>();
        builder.Services.AddScoped<INotificationRealTimeSender, NotificationRealTimeSender>();
        #endregion



        #endregion

        var app = builder.Build();

        #region Configure the HTTP request pipeline.
        // Use forwarded headers to correctly handle client IP addresses when behind a reverse proxy
        app.UseForwardedHeaders();

        app.UseWebSockets();

        app.UseExceptionHandler();

        #region OutputCache Middleware
        app.UseOutputCache();
        #endregion

        #region Versioning
        var apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();
        #endregion

        #region Map Endpoints
        app.MapTypeEndpoints(apiVersionSet);
        app.MapBrandEndpoints(apiVersionSet);
        app.MapUserEndpoints(apiVersionSet);
        app.MapDeliveryMethodEndpoints(apiVersionSet);
        app.MapOrderEndpoints(apiVersionSet);
        app.MapPaymentEndpoints(apiVersionSet);
        app.MapNotificationEndpoints(apiVersionSet);
        #endregion

        #region Data Seeding
        await using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var dbSeeder = scope.ServiceProvider
            .GetRequiredService<DatabaseSeeder>();

        await dbContext.Database.MigrateAsync();
        await dbSeeder.SeedAsync();
        #endregion

        await using var identityScope = app.Services.CreateAsyncScope();
        var identityDbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationIdentityDbContext>();
        var userEntity = identityDbContext.Model
            .FindEntityType(typeof(ApplicationUser));

        Console.WriteLine(
            $"ApplicationUser Table: {userEntity?.GetTableName()}");

        #region Configure Swagger
        app.UseSwagger(); // run middleware that makes documentation file available to use.
        app.UseSwaggerUI();
        #endregion

        app.UseHttpsRedirection();

        #region Serilog Middleware
        app.UseSerilogRequestLogging();

        #endregion

        #region CORS Middleware
        app.UseCors("UnifiedPolicy");
        #endregion

        app.UseStaticFiles(new StaticFileOptions
        {
            OnPrepareResponse = ctx =>
            {
                var path = ctx.Context.Request.Path.Value ?? "";

                if (path.StartsWith("/Images/", StringComparison.OrdinalIgnoreCase))
                {
                    ctx.Context.Response.Headers["Access-Control-Allow-Origin"] = "*";
                    ctx.Context.Response.Headers["Cross-Origin-Resource-Policy"] = "cross-origin";
                    ctx.Context.Response.Headers["Vary"] = "Origin";
                }
            }
        });

        #region IP Security Middleware
        app.UseWhen(
            context => !context.Request.Path.StartsWithSegments("/hubs"),
            appBuilder =>
            {
                appBuilder.UseIpSecurity();
            }
        );
        #endregion

        #region Authentication & Authorization Middleware
        app.UseAuthentication();
        app.UseAuthorization();
        #endregion

        #region RateLimiter Middleware
        app.UseWhen(
            context => !context.Request.Path.StartsWithSegments("/hubs"),
            appBuilder =>
            {
                appBuilder.UseCustomRateLimiter();
                appBuilder.UseRateLimiter();
            }
        );
        #endregion

        #region SerilogUI Middleware
        app.UseSerilogUiAuth();
        app.UseSerilogUi();
        #endregion

        app.MapControllers();

        app.MapHub<NotificationHub>("/hubs/notifications");

        #endregion

        app.Run();
    }
}
