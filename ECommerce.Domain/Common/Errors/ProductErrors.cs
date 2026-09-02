namespace ECommerce.Domain.Common.Errors;
public static class ProductErrors
{
    public static readonly Error NameRequired =
        Error.Validation(
            "Product.NameRequired",
            "Product name is required.");

    public static readonly Error NameTooLong =
        Error.Validation(
            "Product.NameTooLong",
            "Product name cannot exceed 200 characters.");

    public static readonly Error DescriptionRequired =
        Error.Validation(
            "Product.DescriptionRequired",
            "Product description is required.");

    public static readonly Error DescriptionTooLong =
        Error.Validation(
            "Product.DescriptionTooLong",
            "Product description cannot exceed 1000 characters.");

    public static readonly Error InvalidPrice =
        Error.Validation(
            "Product.InvalidPrice",
            "Product price must be greater than zero.");

    public static readonly Error PictureUrlRequired =
        Error.Validation(
            "Product.PictureUrlRequired",
            "Product picture URL is required.");

    public static readonly Error PictureUrlTooLong =
        Error.Validation(
            "Product.PictureUrlTooLong",
            "Product picture URL cannot exceed 500 characters.");

    public static readonly Error InvalidBrandId =
        Error.Validation(
            "Product.InvalidBrandId",
            "Product brand ID must be greater than zero.");

    public static readonly Error InvalidTypeId =
        Error.Validation(
            "Product.InvalidTypeId",
            "Product type ID must be greater than zero.");

    public static readonly Error ProductNotFound =
        Error.NotFound(
            "Product.NotFound",
            "The product was not found.");
}
