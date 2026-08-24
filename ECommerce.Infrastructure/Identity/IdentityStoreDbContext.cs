using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Identity;

public class IdentityStoreDbContext(DbContextOptions<IdentityStoreDbContext> options) :
    IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
{
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("RefreshTokens");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.TokenHash)
                .IsRequired()
                .HasMaxLength(RefreshToken.HashLength);

            // Lookup happens by hash on every refresh call, and a hash must never collide.
            entity.HasIndex(x => x.TokenHash)
                .IsUnique();

            entity.HasIndex(x => x.UserId);

            // Deleting a user must not leave live sessions behind.
            entity.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
