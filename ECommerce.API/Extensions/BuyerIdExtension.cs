using ECommerce.Domain.Errors;
using ECommerce.Domain.Shared;
using System.Security.Claims;

namespace ECommerce.API.Extensions;

public static class BuyerIdExtension
{
    /// <summary>
    /// The header a guest shopper uses to carry a client-generated basket id between requests.
    /// </summary>
    public const string BuyerIdHeader = "X-Buyer-Id";

    /// <summary>
    /// Resolves the basket owner for the current request.
    /// <para>
    /// For an authenticated caller the id always comes from the signed token. The
    /// <c>X-Buyer-Id</c> header is deliberately ignored in that case: honouring it would let
    /// anyone read or mutate another shopper's basket by sending their id.
    /// </para>
    /// </summary>
    public static Result<Guid> GetBuyerId(this HttpContext httpContext)
    {
        if (httpContext.User.Identity?.IsAuthenticated == true)
        {
            var claim = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            return Guid.TryParse(claim, out var userId)
                ? Result<Guid>.Success(userId)
                : Result<Guid>.Failure(BasketErrors.AuthenticatedBuyerIdMissing);
        }

        var header = httpContext.Request.Headers[BuyerIdHeader].ToString();

        return Guid.TryParse(header, out var guestId)
            ? Result<Guid>.Success(guestId)
            : Result<Guid>.Failure(BasketErrors.GuestBuyerIdRequired);
    }
}
