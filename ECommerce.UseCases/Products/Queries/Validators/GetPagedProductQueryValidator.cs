using FluentValidation;

namespace ECommerce.UseCases.Products.Queries.Validators;

public class GetPagedProductQueryValidator : AbstractValidator<GetPagedProductQuery>
{
    private readonly int _pageNumber = 1;
    public GetPagedProductQueryValidator()
    {
        RuleFor(q => q.PageNumber)
            .GreaterThanOrEqualTo(_pageNumber)
            .WithErrorCode("Products.PageNumber.Invalid")
            .WithMessage($"Page number must be at least {_pageNumber}");

        RuleFor(q => q.PageSize)
            .InclusiveBetween(1, 1000)
            .WithErrorCode("Products.PageSize.Invalid")
            .WithMessage("Page Size must be at least between 1 and 1000");
    }
}
