namespace ECommerce.Domain.Errors;

public class ProductTypeErrors
{
    public static readonly Error NotFound =
        Error.NotFound(
            "ProductType.NotFound",
            "Product type was not found.");

    public static readonly Error IdRequired =
        Error.Validation(
            "ProductType.Id.Required",
            "Product type id is required.");

    public static readonly Error NameRequired =
        Error.Validation(
            "ProductType.Name.Required",
            "Product type name is required.");

    public static readonly Error NameAlreadyExists =
        Error.Conflict(
            "ProductType.Name.AlreadyExists",
            "A product type with this name already exists.");
}
