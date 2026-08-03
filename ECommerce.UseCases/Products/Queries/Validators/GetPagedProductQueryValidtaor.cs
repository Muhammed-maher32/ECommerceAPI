using FluentValidation;

namespace ECommerce.UseCases.Products.Queries.Validators;

public class GetPagedProductQueryValidtaor : AbstractValidator<GetPagedProductQuery>
{
    private readonly int _pageNumber = 1;
    public GetPagedProductQueryValidtaor()
    {
        RuleFor(q => q.pageNumber)
            .GreaterThanOrEqualTo(_pageNumber)
            .WithErrorCode("Products.PageNumber.Invalid")
            .WithMessage($"Page number must be at least {_pageNumber}");

        RuleFor(q => q.pageSize)
            .InclusiveBetween(1, 1000)
            .WithErrorCode("Products.PageSize.Invalid")
            .WithMessage("Page Size must be at least between 1 and 1000");
    }
}
