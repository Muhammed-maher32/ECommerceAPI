using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ECommerce.Infrastructure.Persistence.DbContexts;

public class StoreDbContext(DbContextOptions<StoreDbContext> options)
    : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductBrand> Brands => Set<ProductBrand>();
    public DbSet<ProductType> Types => Set<ProductType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StoreDbContext).Assembly);

        ApplySoftDeleteQueryFilter(modelBuilder);
    }

    private static void ApplySoftDeleteQueryFilter(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;

            if (!typeof(BaseEntity).IsAssignableFrom(clrType))
                continue;

            // e =>
            var parameter = Expression.Parameter(clrType, "e");

            // e.IsDeleted
            var property = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));

            // e.IsDeleted == false
            var condition = Expression.Equal(property, Expression.Constant(false));

            // e => e.IsDeleted == false
            var lambda = Expression.Lambda(condition, parameter);

            modelBuilder.Entity(clrType).HasQueryFilter(lambda);
        }
    }
}
