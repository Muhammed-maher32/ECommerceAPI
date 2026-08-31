using ECommerce.UseCases.Orders.Queries.GetOrderById;
using FluentValidation;

namespace ECommerce.UseCases.Orders.Queries.Validators;

public class GetOrderByIdQueryValidator : AbstractValidator<GetOrderByIdQuery>
{
    public GetOrderByIdQueryValidator()
    {
        RuleFor(q => q.OrderId)
            .NotEmpty()
            .WithErrorCode("Order.NotFound")
            .WithMessage("An order id is required.");

        RuleFor(q => q.BuyerId)
            .NotEmpty()
            .WithErrorCode("Order.InvalidBuyerId")
            .WithMessage("A valid buyer id is required.");
    }
}
