using ECommerce.UseCases.Users.Commands.Register;
using FluentValidation;

namespace ECommerce.UseCases.Users.Commands.Validators;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    private const int MaxDisplayNameLength = 100;

    public RegisterCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .WithErrorCode("Users.Email.Required")
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithErrorCode("Users.Email.Invalid")
            .WithMessage("Email must be a valid email address.");

        RuleFor(c => c.Password)
            .Password();

        RuleFor(c => c.DisplayName)
            .MaximumLength(MaxDisplayNameLength)
            .WithErrorCode("Users.DisplayName.TooLong")
            .WithMessage($"Display name must not exceed {MaxDisplayNameLength} characters.")
            .When(c => c.DisplayName is not null);
    }
}
