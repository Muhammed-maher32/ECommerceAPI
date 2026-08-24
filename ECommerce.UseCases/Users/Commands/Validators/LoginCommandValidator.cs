using ECommerce.UseCases.Users.Commands.Login;
using FluentValidation;

namespace ECommerce.UseCases.Users.Commands.Validators;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .WithErrorCode("Users.Email.Required")
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithErrorCode("Users.Email.Invalid")
            .WithMessage("Email must be a valid email address.");

        RuleFor(c => c.Password)
            .NotEmpty()
            .WithErrorCode("Users.Password.Required")
            .WithMessage("Password is required.");
    }
}