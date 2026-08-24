using ECommerce.Domain.Shared;
using ECommerce.UseCases.Shared.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ECommerce.UseCases.Users.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler(
    IIdentityService identityService,
    ILogger<ForgotPasswordCommandHandler> logger) :
    IRequestHandler<ForgotPasswordCommand, Result>
{
    public async Task<Result> Handle(
        ForgotPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var tokenResult = await identityService.GeneratePasswordResetTokenAsync(
            request.Email, cancellationToken);

        if (tokenResult.IsFailure)
        {
            // Deliberately swallowed. Returning the failure would tell the caller which
            // addresses are registered -- account enumeration. Log it, answer success.
            logger.LogInformation(
                "Password reset requested for an address that cannot receive one. {ErrorCode}",
                tokenResult.Error!.Code);

            return Result.Success();
        }

        // TODO: hand the token to the email service (P10). Until that exists this log is
        // the only place the token is visible -- read it from the Development console.
        logger.LogWarning(
            "Password reset token for {Email}: {Token}", request.Email, tokenResult.Value);

        return Result.Success();
    }
}
