using ECommerce.API.Endpoints;
using ECommerce.Infrastructure;
using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.Infrastructure.Persistence.Seeding;
using ECommerce.UseCases;
using Microsoft.EntityFrameworkCore;
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
        #endregion

        var app = builder.Build();

        #region Configure the HTTP request pipeline.
        app.UseExceptionHandler();

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

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.MapProductsEndpoints();

        #endregion

        app.Run();
    }
}
