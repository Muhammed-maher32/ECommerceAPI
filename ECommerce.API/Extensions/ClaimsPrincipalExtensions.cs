using ECommerce.Domain.Errors;
using ECommerce.Domain.Shared;
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
}
