using ECommerce.Domain.Shared;

namespace ECommerce.Domain.Errors;

public class DeliveryMethodErrors
{
    public static readonly Error NotFound =
        Error.NotFound(
            "DeliveryMethod.NotFound",
            "Delivery method was not found.");

    public static readonly Error IdRequired =
        Error.Validation(
            "DeliveryMethod.Id.Required",
            "Delivery method id is required.");

    public static readonly Error NameRequired =
        Error.Validation(
            "DeliveryMethod.Name.Required",
            "Delivery method name is required.");

    public static readonly Error DescriptionRequired =
        Error.Validation(
            "DeliveryMethod.Description.Required",
            "Delivery method description is required.");

    public static readonly Error DeliveryTimeRequired =
        Error.Validation(
            "DeliveryMethod.DeliveryTime.Required",
            "Delivery method delivery time is required.");

    public static readonly Error PriceNegative =
        Error.Validation(
            "DeliveryMethod.Price.Negative",
            "Delivery method price cannot be negative.");
}
