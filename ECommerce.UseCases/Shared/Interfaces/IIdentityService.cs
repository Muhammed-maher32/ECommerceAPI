using ECommerce.Domain.Shared;
using ECommerce.UseCases.Shared.Models;

namespace ECommerce.UseCases.Shared.Interfaces;

public interface IIdentityService
{
    Task<Result<AuthUserSnapshot>> CreateUserAsync(
        string email,
        string password,
        string? displayName,
        CancellationToken cancellationToken = default);

    Task<Result<AuthUserSnapshot>> ValidateCredentialsAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default);

    Task<Result<AuthUserSnapshot>> GetUserByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    Task<Result<AuthUserSnapshot>> GetUserByIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<Result<AuthUserSnapshot>> UpdateProfileAsync(
        Guid userId,
        string? displayName,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<string>> GetRolesAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    // ---------------------------------------------------------------- email confirmation

    /// <summary>
    /// Produces a single-use confirmation token to be emailed to the user.
    /// </summary>
    Task<Result<string>> GenerateEmailConfirmationTokenAsync(
        string email,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Confirms the address only if <paramref //name="token"/> is a valid, unexpired token that
    /// Identity itself issued for this user.
    /// </summary>
    Task<Result> ConfirmEmailAsync(
        string email,
        string token,
        CancellationToken cancellationToken = default);

    Task<bool> IsEmailConfirmedAsync(
        string email,
        CancellationToken cancellationToken = default);

    // ---------------------------------------------------------------- passwords

    /// <summary>
    /// For a signed-in user: requires the current password, so a stolen access token alone
    /// cannot change it.
    /// </summary>
    Task<Result> ChangePasswordAsync(
        Guid userId,
        string currentPassword,
        string newPassword,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Produces a single-use reset token to be emailed to the user.
    /// The calling endpoint must respond identically whether or not the account exists,
    /// otherwise it leaks which emails are registered.
    /// </summary>
    Task<Result<string>> GeneratePasswordResetTokenAsync(
        string email,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Resets the password against a token from <see cref="GeneratePasswordResetTokenAsync"/>
    /// and revokes every refresh token the user holds.
    /// </summary>
    Task<Result> ResetPasswordAsync(
        string email,
        string token,
        string newPassword,
        CancellationToken cancellationToken = default);

    // ---------------------------------------------------------------- refresh tokens

    /// <summary>
    /// Issues a new refresh token. The raw value is returned once; only its hash is stored.
    /// </summary>
    Task<Result<RefreshTokenResult>> IssueRefreshTokenAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates a refresh token, revokes it, and issues a replacement (rotation).
    /// Presenting an already-revoked token is treated as theft: every token the user holds
    /// is revoked and the call fails.
    /// </summary>
    Task<Result<RefreshTokenRotationResult>> RotateRefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default);

    /// <summary>Revokes a single refresh token — use for logout.</summary>
    Task<Result> RevokeRefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default);

    /// <summary>Revokes every refresh token for a user — use for "sign out everywhere".</summary>
    Task<Result> RevokeAllRefreshTokensAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}
