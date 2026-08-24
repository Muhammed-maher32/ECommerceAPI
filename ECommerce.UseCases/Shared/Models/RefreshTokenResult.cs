namespace ECommerce.UseCases.Shared.Models;

/// <summary>
/// A freshly issued refresh token. The raw <paramref name="Token"/> is returned to the caller
/// once and never stored; only its hash is persisted.
/// </summary>
public sealed record RefreshTokenResult(string Token, DateTimeOffset ExpiresAtUtc);

/// <summary>
/// The outcome of rotating a refresh token: who it belongs to, plus the replacement token.
/// </summary>
public sealed record RefreshTokenRotationResult(
    AuthUserSnapshot User,
    RefreshTokenResult RefreshToken);
