using ECommerce.Domain.Shared;
using ECommerce.UseCases.Shared.Interfaces;
using MediatR;

namespace ECommerce.UseCases.Users.Commands.ChangePassword;

public class ChangePasswordCommandHandler(IIdentityService identityService) :
 IRequestHandler<ChangePasswordCommand, Result>
{
    public async Task<Result> Handle(
        ChangePasswordCommand request,
        CancellationToken cancellationToken)
    {
        return await identityService.ChangePasswordAsync(
            request.UserId,
            request.CurrentPassword,
            request.NewPassword,
            cancellationToken);
    }
}