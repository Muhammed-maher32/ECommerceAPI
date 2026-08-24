using ECommerce.UseCases.Users.Commands.ForgotPassword;
using FluentValidation;

namespace ECommerce.UseCases.Users.Commands.Validators;

public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .WithErrorCode("Users.Email.Required")
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithErrorCode("Users.Email.Invalid")
            .WithMessage("Email must be a valid email address.");
    }
}