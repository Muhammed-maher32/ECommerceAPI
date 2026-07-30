using ECommerce.Domain.Entities;

public static class ProductErrors
{
    public static readonly Error NotFound =
        Error.NotFound(
            "Product.NotFound",
            "Product was not found.");

    public static readonly Error NameRequired =
        Error.Validation(
            "Product.Name.Required",
            "Product name is required.");

    public static readonly Error NameTooLong =
        Error.Validation(
            "Product.Name.TooLong",
            $"Product name cannot exceed {Product.MaxNameLength} characters.");

    public static readonly Error NameAlreadyExists =
        Error.Conflict(
            "Product.Name.AlreadyExists",
            "A product with this name already exists.");

    public static readonly Error DescriptionRequired =
        Error.Validation(
            "Product.Description.Required",
            "Product description is required.");

    public static readonly Error DescriptionTooLong =
        Error.Validation(
            "Product.Description.TooLong",
            $"Product description cannot exceed {Product.MaxDescriptionLength} characters.");

    public static readonly Error PictureUrlRequired =
        Error.Validation(
            "Product.PictureUrl.Required",
            "Picture URL is required.");

    public static readonly Error PictureUrlTooLong =
        Error.Validation(
            "Product.PictureUrl.TooLong",
            $"Picture URL cannot exceed {Product.MaxPictureUrlLength} characters.");

    public static readonly Error InvalidPrice =
        Error.Validation(
            "Product.Price.Invalid",
            "Price must be greater than zero.");

    public static readonly Error BrandRequired =
        Error.Validation(
            "Product.Brand.Required",
            "A product brand is required.");

    public static readonly Error TypeRequired =
        Error.Validation(
            "Product.Type.Required",
            "A product type is required.");

    public static readonly Error Deleted =
        Error.Conflict(
            "Product.Deleted",
            "The product has been deleted.");
}