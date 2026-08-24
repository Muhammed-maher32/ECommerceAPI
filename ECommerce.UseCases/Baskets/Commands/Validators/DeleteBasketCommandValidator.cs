using ECommerce.UseCases.Baskets.Commands.ClearBasket;
using FluentValidation;

namespace ECommerce.UseCases.Baskets.Commands.Validators;

public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(c => c.BuyerId)
            .NotEmpty()
            .WithErrorCode("Basket.InvalidBuyerId")
            .WithMessage("A valid buyer id is required.");
    }
}
