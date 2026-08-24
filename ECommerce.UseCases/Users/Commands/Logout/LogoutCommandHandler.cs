using ECommerce.Domain.Shared;
using ECommerce.UseCases.Shared.Interfaces;
using MediatR;

namespace ECommerce.UseCases.Users.Commands.Logout;

public class LogoutCommandHandler(IIdentityService identityService) :
    IRequestHandler<LogoutCommand, Result>
{
    public async Task<Result> Handle(
        LogoutCommand request,
        CancellationToken cancellationToken)
    {
        return await identityService.RevokeRefreshTokenAsync(
            request.RefreshToken, cancellationToken);
    }
}