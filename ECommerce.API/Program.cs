using Asp.Versioning;
using ECommerce.API.Endpoints;
using ECommerce.API.Middlewares.Extensions;
using ECommerce.Infrastructure;
using ECommerce.Infrastructure.Identity;
using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.Infrastructure.Persistence.Seeding;
using ECommerce.UseCases;
using ECommerce.UseCases.Commons.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
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
        #endregion

        builder.Services.AddOutputCache(options =>
        {
            options.AddPolicy("Products", policy =>
            {
                policy.Expire(TimeSpan.FromMinutes(1))
                    .SetVaryByQuery("PageNumber", "PageSize");
            });
        });

        builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = true;
        })
            .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
            .AddDefaultTokenProviders();

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
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"] ?? String.Empty)),
                    ClockSkew = TimeSpan.FromMinutes(3),
                };
            });

        builder.Services.AddAuthorization();

        builder.Services.Configure<IpSecuritySettings>(builder.Configuration.GetSection(nameof(IpSecuritySettings)));

        #region Forwarded Headers Configuration
        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor
                                     | ForwardedHeaders.XForwardedProto;
        });
        #endregion

        #endregion

        var app = builder.Build();

        #region Configure the HTTP request pipeline.
        // Use forwarded headers to correctly handle client IP addresses when behind a reverse proxy
        app.UseForwardedHeaders();

        app.UseExceptionHandler();
        app.UseOutputCache();

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
        #endregion

        #region Data Seeding
        if (app.Environment.IsDevelopment())
        {
            await using var scope = app.Services.CreateAsyncScope();
            var dbContext = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            var dbSeeder = scope.ServiceProvider
                .GetRequiredService<DatabaseSeeder>();

            await dbContext.Database.MigrateAsync();
            await dbSeeder.SeedAsync();
        }
        #endregion

        #region Configure Swagger
        app.UseSwagger(); // run middleware that makes documentation file available to use.
        app.UseSwaggerUI();
        #endregion

        app.UseHttpsRedirection();

        app.UseSerilogRequestLogging();

        #region IP Security Middleware
        app.UseIpSecurity();
        #endregion

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        // app.MapProductsEndpoints();

        #endregion

        app.Run();
    }
}
