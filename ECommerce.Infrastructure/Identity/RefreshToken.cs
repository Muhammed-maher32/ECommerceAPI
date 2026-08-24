namespace ECommerce.Infrastructure.Identity;

/// <summary>
/// A persisted refresh token. Only the SHA-256 hash of the token is stored, for the same reason
/// passwords are hashed: a leaked database must not hand out usable sessions.
/// </summary>
public sealed class RefreshToken
{
    public const int HashLength = 64; // SHA-256 as hex

    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }

    public ApplicationUser User { get; set; } = null!;

    public string TokenHash { get; set; } = null!;

    public DateTimeOffset CreatedAtUtc { get; set; }

    public DateTimeOffset ExpiresAtUtc { get; set; }

    public DateTimeOffset? RevokedAtUtc { get; set; }

    /// <summary>Set when this token was rotated, pointing at its replacement.</summary>
    public Guid? ReplacedByTokenId { get; set; }

    public bool IsRevoked => RevokedAtUtc is not null;

    public bool IsExpired(DateTimeOffset utcNow) => utcNow >= ExpiresAtUtc;

    public bool IsActive(DateTimeOffset utcNow) => !IsRevoked && !IsExpired(utcNow);
}
