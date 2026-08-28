using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        builder.Property(x => x.OrderId)
            .IsRequired();

        // A snapshot, not a relationship: the product may be renamed, re-priced
        // or soft-deleted later, so there is deliberately no FK to Products.
        builder.Property(x => x.ProductId)
            .IsRequired();

        builder.Property(x => x.ProductName)
            .HasMaxLength(OrderItem.MaxProductNameLength)
            .IsRequired();

        builder.Property(x => x.PictureUrl)
            .HasMaxLength(OrderItem.MaxPictureUrlLength)
            .IsRequired();

        builder.Property(x => x.UnitPrice)
            .HasPrecision(18, 2);

        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.Ignore(x => x.LineTotal);

        builder.HasIndex(x => x.OrderId);
    }
}
