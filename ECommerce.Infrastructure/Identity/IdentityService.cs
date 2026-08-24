using ECommerce.Domain.Constants;
using ECommerce.Domain.Errors;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Shared.Interfaces;
using ECommerce.UseCases.Shared.Models;
using ECommerce.UseCases.Shared.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace ECommerce.Infrastructure.Identity;

public sealed class IdentityService(
    UserManager<ApplicationUser> userManager,
    IdentityStoreDbContext dbContext,
    IOptions<JwtSettings> jwtSettings,
    ILogger<IdentityService> logger) : IIdentityService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public async Task<Result<AuthUserSnapshot>> CreateUserAsync(
        string email,
        string password,
        string? displayName,
        CancellationToken cancellationToken = default)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            DisplayName = displayName,
            EmailConfirmed = false
        };

        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            if (result.Errors.Any(e =>
                    e.Code is "DuplicateEmail" or "DuplicateUserName"))
            {
                return Result<AuthUserSnapshot>.Failure(IdentityErrors.EmailAlreadyExists);
            }

            var message = string.Join(" ", result.Errors.Select(e => e.Description));
            return Result<AuthUserSnapshot>.Failure(IdentityErrors.CreateFailed(message));
        }

        // A user with no role gets a token with no role claims and is silently locked out of
        // every protected endpoint, so this failure must not be swallowed.
        var roleResult = await userManager.AddToRoleAsync(user, Roles.User);
        if (!roleResult.Succeeded)
        {
            var message = string.Join(" ", roleResult.Errors.Select(e => e.Description));

            logger.LogError(
                "Created user {Email} but failed to assign the {Role} role: {Errors}",
                email, Roles.User, message);

            // Roll back rather than leave an unusable account behind.
            await userManager.DeleteAsync(user);

            return Result<AuthUserSnapshot>.Failure(IdentityErrors.CreateFailed(message));
        }

        return Result<AuthUserSnapshot>.Success(Snapshot(user));
    }

    public async Task<Result<AuthUserSnapshot>> ValidateCredentialsAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
            return Result<AuthUserSnapshot>.Failure(IdentityErrors.InvalidCredentials);

        // Goes through UserManager so lockout counters and password-hash upgrades still apply.
        var isValid = await userManager.CheckPasswordAsync(user, password);
        if (!isValid)
            return Result<AuthUserSnapshot>.Failure(IdentityErrors.InvalidCredentials);

        // Defers to the configured policy rather than enforcing confirmation unconditionally:
        // SignIn.RequireConfirmedEmail is the single switch, so turning enforcement on once the
        // email service exists is a one-line configuration change and nothing here moves.
        if (userManager.Options.SignIn.RequireConfirmedEmail && !user.EmailConfirmed)
            return Result<AuthUserSnapshot>.Failure(IdentityErrors.EmailNotConfirmed);

        return Result<AuthUserSnapshot>.Success(Snapshot(user));
    }

    public async Task<Result<AuthUserSnapshot>> GetUserByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);

        return user is null
            ? Result<AuthUserSnapshot>.Failure(IdentityErrors.UserNotFound)
            : Result<AuthUserSnapshot>.Success(Snapshot(user));
    }

    public async Task<Result<AuthUserSnapshot>> GetUserByIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());

        return user is null
            ? Result<AuthUserSnapshot>.Failure(IdentityErrors.UserNotFound)
            : Result<AuthUserSnapshot>.Success(Snapshot(user));
    }

    public async Task<Result<AuthUserSnapshot>> UpdateProfileAsync(
        Guid userId,
        string? displayName,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return Result<AuthUserSnapshot>.Failure(IdentityErrors.UserNotFound);

        user.DisplayName = string.IsNullOrWhiteSpace(displayName) ? null : displayName.Trim();

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
            return Result<AuthUserSnapshot>.Failure(Describe(result, IdentityErrors.UpdateFailed));

        return Result<AuthUserSnapshot>.Success(Snapshot(user));
    }

    public async Task<IReadOnlyList<string>> GetRolesAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return [];

        var roles = await userManager.GetRolesAsync(user);
        return [.. roles];
    }

    // ------------------------------------------------------------------ email confirmation

    public async Task<Result<string>> GenerateEmailConfirmationTokenAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
            return Result<string>.Failure(IdentityErrors.UserNotFound);

        if (user.EmailConfirmed)
            return Result<string>.Failure(IdentityErrors.EmailAlreadyConfirmed);

        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        return Result<string>.Success(token);
    }

    public async Task<Result> ConfirmEmailAsync(
        string email,
        string token,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
            return Result.Failure(IdentityErrors.UserNotFound);

        if (user.EmailConfirmed)
            return Result.Failure(IdentityErrors.EmailAlreadyConfirmed);

        // Identity verifies the token was issued for this user and has not expired;
        // setting EmailConfirmed directly would let anyone confirm anyone's address.
        var result = await userManager.ConfirmEmailAsync(user, token);

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(IdentityErrors.InvalidToken);
    }

    public async Task<bool> IsEmailConfirmedAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        return user?.EmailConfirmed ?? false;
    }

    // ------------------------------------------------------------------ passwords

    public async Task<Result> ChangePasswordAsync(
        Guid userId,
        string currentPassword,
        string newPassword,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return Result.Failure(IdentityErrors.UserNotFound);

        var result = await userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        if (!result.Succeeded)
        {
            // A wrong current password is an authentication failure, not a validation error.
            if (result.Errors.Any(e => e.Code == "PasswordMismatch"))
                return Result.Failure(IdentityErrors.InvalidCredentials);

            return Result.Failure(Describe(result, IdentityErrors.UpdateFailed));
        }

        await RevokeAllRefreshTokensAsync(userId, cancellationToken);

        return Result.Success();
    }

    public async Task<Result<string>> GeneratePasswordResetTokenAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
            return Result<string>.Failure(IdentityErrors.UserNotFound);

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        return Result<string>.Success(token);
    }

    public async Task<Result> ResetPasswordAsync(
        string email,
        string token,
        string newPassword,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
            return Result.Failure(IdentityErrors.UserNotFound);

        var result = await userManager.ResetPasswordAsync(user, token, newPassword);
        if (!result.Succeeded)
        {
            if (result.Errors.Any(e => e.Code == "InvalidToken"))
                return Result.Failure(IdentityErrors.InvalidToken);

            return Result.Failure(Describe(result, IdentityErrors.UpdateFailed));
        }

        // Whoever reset the password owns the account now; older sessions must not survive.
        await RevokeAllRefreshTokensAsync(user.Id, cancellationToken);

        return Result.Success();
    }

    // ------------------------------------------------------------------ refresh tokens

    public async Task<Result<RefreshTokenResult>> IssueRefreshTokenAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var exists = await userManager.FindByIdAsync(userId.ToString());
        if (exists is null)
            return Result<RefreshTokenResult>.Failure(IdentityErrors.UserNotFound);

        var (entity, raw) = BuildRefreshToken(userId);

        dbContext.RefreshTokens.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<RefreshTokenResult>.Success(
            new RefreshTokenResult(raw, entity.ExpiresAtUtc));
    }

    public async Task<Result<RefreshTokenRotationResult>> RotateRefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return Result<RefreshTokenRotationResult>.Failure(IdentityErrors.InvalidRefreshToken);

        var hash = HashToken(refreshToken);

        var stored = await dbContext.RefreshTokens
            .FirstOrDefaultAsync(t => t.TokenHash == hash, cancellationToken);

        if (stored is null)
            return Result<RefreshTokenRotationResult>.Failure(IdentityErrors.InvalidRefreshToken);

        var utcNow = DateTimeOffset.UtcNow;

        // A revoked token coming back means it was captured: the legitimate holder already
        // rotated it. Kill every session for this user rather than just refusing this one.
        if (stored.IsRevoked)
        {
            logger.LogWarning(
                "Refresh token reuse detected for user {UserId}; revoking all sessions.",
                stored.UserId);

            await RevokeAllRefreshTokensAsync(stored.UserId, cancellationToken);

            return Result<RefreshTokenRotationResult>.Failure(
                IdentityErrors.RefreshTokenReuseDetected);
        }

        if (stored.IsExpired(utcNow))
            return Result<RefreshTokenRotationResult>.Failure(IdentityErrors.RefreshTokenExpired);

        var user = await userManager.FindByIdAsync(stored.UserId.ToString());
        if (user is null)
            return Result<RefreshTokenRotationResult>.Failure(IdentityErrors.UserNotFound);

        var (replacement, raw) = BuildRefreshToken(stored.UserId);

        stored.RevokedAtUtc = utcNow;
        stored.ReplacedByTokenId = replacement.Id;

        dbContext.RefreshTokens.Add(replacement);

        // One SaveChanges, so revoking the old token and storing the new one either both
        // happen or neither does.
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<RefreshTokenRotationResult>.Success(
            new RefreshTokenRotationResult(
                Snapshot(user),
                new RefreshTokenResult(raw, replacement.ExpiresAtUtc)));
    }

    public async Task<Result> RevokeRefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return Result.Failure(IdentityErrors.InvalidRefreshToken);

        var hash = HashToken(refreshToken);

        var stored = await dbContext.RefreshTokens
            .FirstOrDefaultAsync(t => t.TokenHash == hash, cancellationToken);

        if (stored is null)
            return Result.Failure(IdentityErrors.InvalidRefreshToken);

        if (stored.IsRevoked)
            return Result.Success(); // idempotent: logging out twice is not an error

        stored.RevokedAtUtc = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> RevokeAllRefreshTokensAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var utcNow = DateTimeOffset.UtcNow;

        await dbContext.RefreshTokens
            .Where(t => t.UserId == userId && t.RevokedAtUtc == null)
            .ExecuteUpdateAsync(
                s => s.SetProperty(t => t.RevokedAtUtc, utcNow),
                cancellationToken);

        return Result.Success();
    }

    // ------------------------------------------------------------------ helpers

    /// <summary>
    /// Builds an unsaved token entity plus the raw value the caller must return to the client.
    /// The caller decides when to persist, so a rotation can revoke and replace atomically.
    /// </summary>
    private (RefreshToken Entity, string RawToken) BuildRefreshToken(Guid userId)
    {
        var raw = GenerateSecureToken();
        var utcNow = DateTimeOffset.UtcNow;

        var entity = new RefreshToken
        {
            UserId = userId,
            TokenHash = HashToken(raw),
            CreatedAtUtc = utcNow,
            ExpiresAtUtc = utcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays)
        };

        return (entity, raw);
    }

    private static AuthUserSnapshot Snapshot(ApplicationUser user) =>
        new(user.Id, user.Email!, user.DisplayName);

    private static Error Describe(IdentityResult result, Func<string, Error> factory) =>
        factory(string.Join(" ", result.Errors.Select(e => e.Description)));

    /// <summary>256 bits of entropy, URL-safe so it can travel in a header or cookie.</summary>
    private static string GenerateSecureToken() =>
        Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');

    private static string HashToken(string token) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
}
