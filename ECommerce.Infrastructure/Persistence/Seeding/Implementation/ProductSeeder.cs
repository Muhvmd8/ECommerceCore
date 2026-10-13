using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.Infrastructure.Persistence.Seeding.Abstraction;
using ECommerce.Infrastructure.Persistence.Seeding.Models;
using ECommerce.UseCases.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ECommerce.Infrastructure.Persistence.Seeding.Implementation;

public class ProductSeeder(
    IWebHostEnvironment environment,
    ApplicationDbContext context,
    IAttachmentService attachmentService,
    ILogger<ProductSeeder> logger) : IDataSeeder
{
    public int Order => 3; // after brands and types

    public async Task SeedDataAsync(CancellationToken ct = default)
        => await JsonSeeder.SeedIfEmptyAsync<Product, int, ProductSeederModel>(
            context.Set<Product>(),
            "products.json",
            MapAsync,
            ct);

    private async Task<Product?> MapAsync(ProductSeederModel model, CancellationToken ct)
    {
        var pictureUrl = await UploadImageAsync(model.PictureUrl);
        if (pictureUrl is null)
        {
            logger.LogWarning("Skipping '{Product}': image missing or upload failed", model.Name);
            return null;
        }

        return Product.Create(
            model.Name,
            model.Description,
            model.Price,
            pictureUrl,
            model.ProductBrandId,
            model.ProductTypeId)
                    .Value;
    }

    private async Task<string?> UploadImageAsync(string fileName)
    {
        var path = Path.Combine(environment.WebRootPath, "Images", "Products", fileName);
        if (!File.Exists(path)) return null;

        await using var stream = File.OpenRead(path);

        IFormFile formFile = new FormFile(stream, 0, stream.Length, "file", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = GetContentType(fileName)
        };

        var result = await attachmentService.UploadAttachmentAsync(formFile);
        return result.IsSuccess ? result.Value : null;
    }

    private static string GetContentType(string fileName) =>
        Path.GetExtension(fileName).ToLowerInvariant() switch
        {
            ".png" => "image/png",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".webp" => "image/webp",
            _ => "application/octet-stream"
        };
}