using ECommerce.Domain.Entities;
using ECommerce.UseCases.Baskets.Commands.AddBasketItem;
using FluentValidation;

namespace ECommerce.UseCases.Baskets.Commands.Validators;

public class AddItemToBasketCommandValidator : AbstractValidator<AddItemToBasketCommand>
{
    public AddItemToBasketCommandValidator()
    {
        RuleFor(c => c.BuyerId)
            .NotEmpty()
            .WithErrorCode("Basket.InvalidBuyerId")
            .WithMessage("A valid buyer id is required.");

        RuleFor(c => c.ProductId)
            .NotEmpty()
            .WithErrorCode("Basket.InvalidProductId")
            .WithMessage("Product id is required.");

        // An increment, not an absolute quantity -- the entity rejects a total above
        // MaxQuantity, so only the step itself is checked here.
        RuleFor(c => c.Quantity)
            .InclusiveBetween(BasketItem.MinQuantity, BasketItem.MaxQuantity)
            .WithErrorCode("Basket.InvalidQuantityIncrement")
            .WithMessage(
                $"Quantity must be between {BasketItem.MinQuantity} and {BasketItem.MaxQuantity}.");
    }
}
