using ECommerce.UseCases.Orders.Queries.GetOrders;
using FluentValidation;

namespace ECommerce.UseCases.Orders.Queries.Validators;

public class GetOrdersQueryValidator : AbstractValidator<GetOrdersQuery>
{
    public GetOrdersQueryValidator()
    {
        RuleFor(q => q.BuyerId)
            .NotEmpty()
            .WithErrorCode("Order.InvalidBuyerId")
            .WithMessage("A valid buyer id is required.");
    }
}
