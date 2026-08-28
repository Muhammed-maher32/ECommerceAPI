using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Shared;

namespace ECommerce.Domain.Errors;

public class OrderErrors
{
    public static readonly Error InvalidBuyerId =
        Error.Validation(
            "Order.InvalidBuyerId",
            "A valid buyer id is required.");

    public static readonly Error InvalidBuyerEmail =
        Error.Validation(
            "Order.InvalidBuyerEmail",
            "Buyer email is required.");

    public static readonly Error ShippingAddressRequired =
        Error.Validation(
            "Order.ShippingAddressRequired",
            "A shipping address is required.");

    public static readonly Error InvalidRecipientName =
        Error.Validation(
            "Order.InvalidRecipientName",
            "Recipient first and last name are required.");

    public static readonly Error InvalidPhone =
        Error.Validation(
            "Order.InvalidPhone",
            "A recipient phone number is required.");

    public static readonly Error InvalidLocation =
        Error.Validation(
            "Order.InvalidLocation",
            "Country, city and street are required.");

    public static readonly Error InvalidPostalCode =
        Error.Validation(
            "Order.InvalidPostalCode",
            "Postal code is required.");

    public static readonly Error InvalidDeliveryMethod =
        Error.Validation(
            "Order.InvalidDeliveryMethod",
            "A delivery method name is required.");

    public static readonly Error InvalidDeliveryPrice =
        Error.Validation(
            "Order.InvalidDeliveryPrice",
            "Delivery price cannot be negative.");

    public static readonly Error EmptyOrder =
        Error.Validation(
            "Order.EmptyOrder",
            "An order must contain at least one item.");

    public static readonly Error DuplicateOrderItem =
        Error.Validation(
            "Order.DuplicateOrderItem",
            "The same product cannot appear twice in one order.");

    public static readonly Error InvalidProductId =
        Error.Validation(
            "Order.InvalidProductId",
            "Product id is required.");

    public static readonly Error InvalidProductName =
        Error.Validation(
            "Order.InvalidProductName",
            "Product name is required.");

    public static readonly Error InvalidPictureUrl =
        Error.Validation(
            "Order.InvalidPictureUrl",
            "Product picture URL is required.");

    public static readonly Error InvalidUnitPrice =
        Error.Validation(
            "Order.InvalidUnitPrice",
            "Unit price must be greater than zero.");

    public static readonly Error InvalidQuantity =
        Error.Validation(
            "Order.InvalidQuantity",
            $"Quantity must be between {OrderItem.MinQuantity} and {OrderItem.MaxQuantity}.");

    public static readonly Error InvalidPaymentIntentId =
        Error.Validation(
            "Order.InvalidPaymentIntentId",
            "A payment intent id is required.");

    public static readonly Error PaymentIntentRequired =
        Error.Validation(
            "Order.PaymentIntentRequired",
            "The order cannot be marked as paid before a payment intent is attached.");

    public static readonly Error PaymentIntentAlreadySet =
        Error.Conflict(
            "Order.PaymentIntentAlreadySet",
            "This order is already linked to a different payment intent.");

    public static readonly Error NotFound =
        Error.NotFound(
            "Order.NotFound",
            "No order was found.");

    public static readonly Error NotOwnedByBuyer =
        Error.Forbidden(
            "Order.NotOwnedByBuyer",
            "This order belongs to a different buyer.");

    // Transition failures are conflicts, not validation errors: the payload was
    // fine, the aggregate is simply not in a state that allows the move.
    public static Error CannotMarkAsPaid(OrderStatus current)
        => Transition("Order.CannotMarkAsPaid", current, "paid");

    public static Error CannotMarkAsFailed(OrderStatus current)
        => Transition("Order.CannotMarkAsFailed", current, "payment failed");

    public static Error CannotShip(OrderStatus current)
        => Transition("Order.CannotShip", current, "shipped");

    public static Error CannotDeliver(OrderStatus current)
        => Transition("Order.CannotDeliver", current, "delivered");

    public static Error CannotCancel(OrderStatus current)
        => Transition("Order.CannotCancel", current, "cancelled");

    private static Error Transition(string code, OrderStatus current, string target)
        => Error.Conflict(
            code,
            $"An order in status {current} cannot be marked as {target}.");
}
