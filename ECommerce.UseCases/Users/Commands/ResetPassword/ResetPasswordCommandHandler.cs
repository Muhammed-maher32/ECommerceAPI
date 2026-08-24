using ECommerce.Domain.Shared;
using ECommerce.UseCases.Shared.Interfaces;
using MediatR;

namespace ECommerce.UseCases.Users.Commands.ResetPassword;

public class ResetPasswordCommandHandler(IIdentityService identityService) :
    IRequestHandler<ResetPasswordCommand, Result>
{
    public async Task<Result> Handle(
        ResetPasswordCommand request,
        CancellationToken cancellationToken)
    {
        // The service also revokes every refresh token the user holds.
        return await identityService.ResetPasswordAsync(
            request.Email,
            request.Token,
            request.NewPassword,
            cancellationToken);
    }
}
