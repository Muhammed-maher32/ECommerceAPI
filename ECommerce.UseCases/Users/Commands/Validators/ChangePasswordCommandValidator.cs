using ECommerce.UseCases.Users.Commands.ChangePassword;
using FluentValidation;

namespace ECommerce.UseCases.Users.Commands.Validators;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(c => c.UserId)
            .NotEmpty()
            .WithErrorCode("Users.UserId.Required")
            .WithMessage("User id is required.");

        RuleFor(c => c.CurrentPassword)
            .NotEmpty()
            .WithErrorCode("Users.Password.Required")
            .WithMessage("Current password is required.");

        RuleFor(c => c.NewPassword)
            .Password();

        RuleFor(c => c.NewPassword)
            .NotEqual(c => c.CurrentPassword)
            .WithErrorCode("Users.Password.Unchanged")
            .WithMessage("The new password must differ from the current one.");
    }
}