using ECommerce.Domain.Shared;
using ECommerce.UseCases.Shared.Interfaces;
using ECommerce.UseCases.Users.Dtos;
using MediatR;

namespace ECommerce.UseCases.Users.Commands.Refresh;

public class RefreshCommandHandler
    (IIdentityService identityService,
    IJwtTokenGenerator jwtTokenGenerator) :
    IRequestHandler<RefreshCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        // Rotation revokes the presented token and issues a replacement in one step;
        // reuse of an already-revoked token fails here and kills every session.
        var rotation = await identityService.RotateRefreshTokenAsync(
            request.RefreshToken, cancellationToken);

        if (rotation.IsFailure)
            return Result<AuthResponse>.Failure(rotation.Error!);

        var user = rotation.Value.User;

        var roles = await identityService.GetRolesAsync(user.UserId, cancellationToken);

        var accessToken = jwtTokenGenerator.GenerateToken(
            user.UserId, user.Email, user.DisplayName, roles);

        // The replacement refresh token came from the rotation itself -- do not issue
        // a second one, or the user ends up holding two valid tokens.
        return Result<AuthResponse>.Success(new AuthResponse(
            accessToken.AccessToken,
            accessToken.ExpiresAtUtc,
            rotation.Value.RefreshToken.Token));
    }
}
