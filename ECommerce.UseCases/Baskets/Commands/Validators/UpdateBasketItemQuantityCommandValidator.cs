using ECommerce.Domain.Entities;
using ECommerce.UseCases.Baskets.Commands.UpdateBasketItemQuantity;
using FluentValidation;

namespace ECommerce.UseCases.Baskets.Commands.Validators;

public class UpdateBasketItemQuantityCommandValidator
    : AbstractValidator<UpdateBasketItemQuantityCommand>
{
    public UpdateBasketItemQuantityCommandValidator()
    {
        RuleFor(c => c.BuyerId)
            .NotEmpty()
            .WithErrorCode("Basket.InvalidBuyerId")
            .WithMessage("A valid buyer id is required.");

        RuleFor(c => c.ProductId)
            .NotEmpty()
            .WithErrorCode("Basket.InvalidProductId")
            .WithMessage("Product id is required.");

        RuleFor(c => c.Quantity)
            .InclusiveBetween(BasketItem.MinQuantity, BasketItem.MaxQuantity)
            .WithErrorCode("Basket.InvalidQuantity")
            .WithMessage(
                $"Quantity must be between {BasketItem.MinQuantity} and {BasketItem.MaxQuantity}.");
    }
}
