using ECommerce.Domain.Shared;

namespace ECommerce.Domain.Errors;

public class ProductBrandErrors
{
    public static readonly Error NotFound =
        Error.NotFound(
            "ProductBrand.NotFound",
            "Product brand was not found.");

    public static readonly Error IdRequired =
        Error.Validation(
            "ProductBrand.Id.Required",
            "Product brand id is required.");

    public static readonly Error NameRequired =
        Error.Validation(
            "ProductBrand.Name.Required",
            "Product brand name is required.");

    public static readonly Error NameAlreadyExists =
        Error.Conflict(
            "ProductBrand.Name.AlreadyExists",
            "A product brand with this name already exists.");
}
