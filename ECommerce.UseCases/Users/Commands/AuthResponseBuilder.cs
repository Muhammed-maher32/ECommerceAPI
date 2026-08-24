using ECommerce.Domain.Shared;
using ECommerce.UseCases.Shared.Interfaces;
using ECommerce.UseCases.Shared.Models;
using ECommerce.UseCases.Users.Dtos;

namespace ECommerce.UseCases.Users.Commands;

internal static class AuthResponseBuilder
{
    public static async Task<Result<AuthResponse>> BuildAsync(
        IIdentityService identityService,
        IJwtTokenGenerator jwtTokenGenerator,
        AuthUserSnapshot user,
        CancellationToken cancellationToken)
    {
        var roles = await identityService.GetRolesAsync(user.UserId, cancellationToken);

        var accessToken = jwtTokenGenerator.GenerateToken(
            user.UserId, user.Email, user.DisplayName, roles);

        var refreshResult = await identityService.IssueRefreshTokenAsync(
            user.UserId, cancellationToken);

        if (refreshResult.IsFailure)
            return Result<AuthResponse>.Failure(refreshResult.Error!);

        return Result<AuthResponse>.Success(new AuthResponse(
            accessToken.AccessToken,
            accessToken.ExpiresAtUtc,
            refreshResult.Value.Token));
    }
}