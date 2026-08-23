using ECommerce.Domain.Constants;
using ECommerce.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ECommerce.Infrastructure.Persistence.Seeding;

public sealed class IdentitySeeder(
    RoleManager<ApplicationRole> roleManager,
    UserManager<ApplicationUser> userManager,
    IConfiguration config,
    ILogger<IdentitySeeder> logger) : IDataSeeder
{
    public int Order => 0;

    public async Task SeedAsync(CancellationToken ct = default)
    {
        await SeedRolesAsync(ct);
        await SeedSuperAdminAsync(ct);
    }

    private async Task SeedRolesAsync(CancellationToken ct)
    {
        if (await roleManager.Roles.AnyAsync(ct))
            return;

        foreach (var roleName in Roles.All)
        {
            await roleManager.CreateAsync(new ApplicationRole(roleName)
            {
                Description = $"{roleName} system role"
            });
        }
    }

    private async Task SeedSuperAdminAsync(CancellationToken ct)
    {
        var section = config.GetSection("Seed:SuperAdmin");
        var email = section["Email"];
        var password = section["Password"];

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            logger.LogWarning(
                "Skipping super admin seeding: Seed:SuperAdmin:Email or :Password is not configured.");
            return;
        }

        var existing = await userManager.FindByEmailAsync(email);

        if (existing is null)
        {
            await CreateSuperAdminAsync(email, password, section["DisplayName"]);
            return;
        }

        // The account outlives the configured password, so a rotated secret would
        // otherwise never take effect. Treat configuration as the source of truth.
        // Safe because seeding only runs in Development (see Program.cs).
        var resetToken = await userManager.GeneratePasswordResetTokenAsync(existing);
        var resetResult = await userManager.ResetPasswordAsync(existing, resetToken, password);

        if (!resetResult.Succeeded)
        {
            logger.LogError(
                "Failed to reset the super admin password for {Email}: {Errors}",
                email,
                Describe(resetResult));
            return;
        }

        await EnsureSuperAdminRoleAsync(existing, email);

        logger.LogInformation("Super admin {Email} password synchronised with configuration.", email);
    }

    private async Task CreateSuperAdminAsync(string email, string password, string? displayName)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            DisplayName = displayName ?? "Super Admin"
        };

        var createResult = await userManager.CreateAsync(user, password);

        if (!createResult.Succeeded)
        {
            // Previously swallowed: a password that violates the configured Identity
            // policy left no super admin and no trace of why.
            logger.LogError(
                "Failed to create the super admin {Email}: {Errors}",
                email,
                Describe(createResult));
            return;
        }

        await EnsureSuperAdminRoleAsync(user, email);

        logger.LogInformation("Seeded super admin {Email}.", email);
    }

    private async Task EnsureSuperAdminRoleAsync(ApplicationUser user, string email)
    {
        if (await userManager.IsInRoleAsync(user, Roles.SuperAdmin))
            return;

        var roleResult = await userManager.AddToRoleAsync(user, Roles.SuperAdmin);

        if (!roleResult.Succeeded)
        {
            logger.LogError(
                "Failed to add {Email} to the {Role} role: {Errors}",
                email,
                Roles.SuperAdmin,
                Describe(roleResult));
        }
    }

    private static string Describe(IdentityResult result) =>
        string.Join("; ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
}
