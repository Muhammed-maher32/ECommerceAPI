using ECommerce.Domain.Entities;
using ECommerce.UseCases.Orders.Commands.CreateOrder;
using FluentValidation;

namespace ECommerce.UseCases.Orders.Commands.Validators;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(c => c.BuyerId)
            .NotEmpty()
            .WithErrorCode("Order.InvalidBuyerId")
            .WithMessage("A valid buyer id is required.");

        RuleFor(c => c.BuyerEmail)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(Order.MaxEmailLength)
            .WithErrorCode("Order.InvalidBuyerEmail")
            .WithMessage("A valid buyer email is required.");

        RuleFor(c => c.DeliveryMethodId)
            .NotEmpty()
            .WithErrorCode("Order.InvalidDeliveryMethod")
            .WithMessage("A delivery method id is required.");

        RuleFor(c => c.ShipToAddress)
            .NotNull()
            .WithErrorCode("Order.ShippingAddressRequired")
            .WithMessage("A shipping address is required.");

        // Lengths only. Whether the values are *present* is the entity's rule, and
        // ShippingAddress.Create enforces it again on the way in.
        When(c => c.ShipToAddress is not null, () =>
        {
            RuleFor(c => c.ShipToAddress.RecipientFirstName)
                .NotEmpty()
                .MaximumLength(ShippingAddress.MaxNameLength)
                .WithErrorCode("Order.InvalidRecipientName")
                .WithMessage("Recipient first and last name are required.");

            RuleFor(c => c.ShipToAddress.RecipientLastName)
                .NotEmpty()
                .MaximumLength(ShippingAddress.MaxNameLength)
                .WithErrorCode("Order.InvalidRecipientName")
                .WithMessage("Recipient first and last name are required.");

            RuleFor(c => c.ShipToAddress.PhoneNumber)
                .NotEmpty()
                .MaximumLength(ShippingAddress.MaxPhoneLength)
                .WithErrorCode("Order.InvalidPhone")
                .WithMessage("A recipient phone number is required.");

            RuleFor(c => c.ShipToAddress.Country)
                .NotEmpty()
                .MaximumLength(ShippingAddress.MaxCountryLength)
                .WithErrorCode("Order.InvalidLocation")
                .WithMessage("Country, city and street are required.");

            RuleFor(c => c.ShipToAddress.City)
                .NotEmpty()
                .MaximumLength(ShippingAddress.MaxCityLength)
                .WithErrorCode("Order.InvalidLocation")
                .WithMessage("Country, city and street are required.");

            RuleFor(c => c.ShipToAddress.Street)
                .NotEmpty()
                .MaximumLength(ShippingAddress.MaxStreetLength)
                .WithErrorCode("Order.InvalidLocation")
                .WithMessage("Country, city and street are required.");

            RuleFor(c => c.ShipToAddress.PostalCode)
                .NotEmpty()
                .MaximumLength(ShippingAddress.MaxPostalCodeLength)
                .WithErrorCode("Order.InvalidPostalCode")
                .WithMessage("Postal code is required.");
        });
    }
}
