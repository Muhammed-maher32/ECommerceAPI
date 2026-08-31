using ECommerce.Domain.Errors;
using ECommerce.Domain.Shared;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace ECommerce.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Reads the caller's id from the access token. The value comes from the signed
    /// token and nowhere else -- never from a route, query string or body.
    /// </summary>
    public static Result<Guid> GetUserId(this ClaimsPrincipal principal)
    {
        var raw = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(raw, out var userId)
            ? Result<Guid>.Success(userId)
            : Result<Guid>.Failure(IdentityErrors.InvalidCredentials);
    }

    /// <summary>
    /// Reads the caller's email from the access token. Same rule as <see cref="GetUserId"/>:
    /// the order records the address the token was issued for, not one the body supplied.
    /// </summary>
    public static Result<string> GetEmail(this ClaimsPrincipal principal)
    {
        var email = principal.FindFirstValue(ClaimTypes.Email)
            ?? principal.FindFirstValue(JwtRegisteredClaimNames.Email);

        return string.IsNullOrWhiteSpace(email)
            ? Result<string>.Failure(IdentityErrors.InvalidCredentials)
            : Result<string>.Success(email);
    }
}
