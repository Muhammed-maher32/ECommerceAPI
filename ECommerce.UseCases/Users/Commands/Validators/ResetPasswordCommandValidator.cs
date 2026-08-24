using ECommerce.UseCases.Users.Commands.ResetPassword;
using FluentValidation;

namespace ECommerce.UseCases.Users.Commands.Validators;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .WithErrorCode("Users.Email.Required")
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithErrorCode("Users.Email.Invalid")
            .WithMessage("Email must be a valid email address.");

        RuleFor(c => c.Token)
            .NotEmpty()
            .WithErrorCode("Users.Token.Required")
            .WithMessage("Reset token is required.");

        RuleFor(c => c.NewPassword)
            .Password();
    }
}