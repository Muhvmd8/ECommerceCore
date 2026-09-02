using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities.Common;

namespace ECommerce.Domain.Entities.ProductModule;

public class Product : BaseEntity<int>
{

    #region Fields
    private const int MaxNameLength = 200;
    private const int MaxDescriptionLength = 1000;
    private const int MaxPictureUrlLength = 500;
    #endregion

    #region Properties
    public string Name { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public decimal Price { get; private set; }
    public string PictureUrl { get; private set; } = default!;

    public int ProductBrandId { get; private set; }
    public ProductBrand ProductBrand { get; private set; } = default!;

    public int ProductTypeId { get; private set; }
    public ProductType ProductType { get; private set; } = default!;
    #endregion

    #region Comstructor
    private Product(
        string name,
        string description,
        decimal price,
        string pictureUrl,
        int productBrandId,
        int productTypeId)
    {
        Name = name;
        Description = description;
        Price = price;
        PictureUrl = pictureUrl;
        ProductBrandId = productBrandId;
        ProductTypeId = productTypeId;
    }
    #endregion

    #region Factory Method
    public static Result<Product> Create(
        string name,
        string description,
        decimal price,
        string pictureUrl,
        int productBrandId,
        int productTypeId)
    {

        var nameResult = SetName(name);
        if (nameResult.IsFailure)
            return Result<Product>.Failure(nameResult.Error!);

        var descriptionResult = SetDescription(description);
        if (descriptionResult.IsFailure)
            return Result<Product>.Failure(descriptionResult.Error!);

        var priceResult = SetPrice(price);
        if (priceResult.IsFailure)
            return Result<Product>.Failure(priceResult.Error!);

        var pictureUrlResult = SetPictureUrl(pictureUrl);
        if (pictureUrlResult.IsFailure)
            return Result<Product>.Failure(pictureUrlResult.Error!);

        var brandResult = SetProductBrandId(productBrandId);
        if (brandResult.IsFailure)
            return Result<Product>.Failure(brandResult.Error!);

        var typeResult = SetProductTypeId(productTypeId);
        if (typeResult.IsFailure)
            return Result<Product>.Failure(typeResult.Error!);

        return Result<Product>.Success(
            new Product(
                name.Trim(),
                description.Trim(),
                price,
                pictureUrl.Trim(),
                productBrandId,
                productTypeId));
    }
    private static Result SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(ProductErrors.NameRequired);

        if (name.Length > MaxNameLength)
            return Result.Failure(ProductErrors.NameTooLong);

        return Result.Success();
    }
    private static Result SetDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure(ProductErrors.DescriptionRequired);

        if (description.Length > MaxDescriptionLength)
            return Result.Failure(ProductErrors.DescriptionTooLong);

        return Result.Success();
    }
    private static Result SetPrice(decimal price)
        => price > 0
            ? Result.Success()
            : Result.Failure(ProductErrors.InvalidPrice);
    private static Result SetPictureUrl(string pictureUrl)
    {
        if (string.IsNullOrWhiteSpace(pictureUrl))
            return Result.Failure(ProductErrors.PictureUrlRequired);

        if (pictureUrl.Length > MaxPictureUrlLength)
            return Result.Failure(ProductErrors.PictureUrlTooLong);

        return Result.Success();
    }
    private static Result SetProductBrandId(int productBrandId)
        => productBrandId > 0
            ? Result.Success()
            : Result.Failure(ProductErrors.InvalidBrandId);
    private static Result SetProductTypeId(int productTypeId)
        => productTypeId > 0
            ? Result.Success()
            : Result.Failure(ProductErrors.InvalidTypeId);
    #endregion
}