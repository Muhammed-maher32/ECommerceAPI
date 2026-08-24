using ECommerce.UseCases.Baskets.Commands.RemoveBasketItem;
using FluentValidation;

namespace ECommerce.UseCases.Baskets.Commands.Validators;

public class RemoveItemFromBasketCommandValidator
    : AbstractValidator<RemoveItemFromBasketCommand>
{
    public RemoveItemFromBasketCommandValidator()
    {
        RuleFor(c => c.BuyerId)
            .NotEmpty()
            .WithErrorCode("Basket.InvalidBuyerId")
            .WithMessage("A valid buyer id is required.");

        RuleFor(c => c.ProductId)
            .NotEmpty()
            .WithErrorCode("Basket.InvalidProductId")
            .WithMessage("Product id is required.");
    }
}
