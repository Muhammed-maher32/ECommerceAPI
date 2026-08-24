using FluentValidation;

namespace ECommerce.UseCases.Users.Commands.Validators;

/// <summary>
/// The single definition of what a valid password looks like. Every command that accepts a
/// new password reuses this, so the API and ASP.NET Identity can never disagree about the
/// policy — see the matching <c>options.Password</c> block in
/// <c>ECommerce.API.DependencyInjection</c>.
/// </summary>
public static class PasswordRules
{
    public const int MinLength = 8;

    public static IRuleBuilderOptions<T, string> Password<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
                .WithErrorCode("Users.Password.Required")
                .WithMessage("Password is required.")
            .MinimumLength(MinLength)
                .WithErrorCode("Users.Password.TooShort")
                .WithMessage($"Password must be at least {MinLength} characters.")
            .Matches("[A-Z]")
                .WithErrorCode("Users.Password.MissingUppercase")
                .WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]")
                .WithErrorCode("Users.Password.MissingLowercase")
                .WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"\d")
                .WithErrorCode("Users.Password.MissingDigit")
                .WithMessage("Password must contain at least one digit.")
            .Matches(@"[^a-zA-Z0-9]")
                .WithErrorCode("Users.Password.MissingSymbol")
                .WithMessage("Password must contain at least one non-alphanumeric character.");
    }
}
