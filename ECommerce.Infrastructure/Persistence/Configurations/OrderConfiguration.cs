using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.Property(x => x.BuyerId)
            .IsRequired();

        builder.Property(x => x.BuyerEmail)
            .HasMaxLength(Order.MaxEmailLength)
            .IsRequired();

        builder.Property(x => x.OrderDate)
            .IsRequired();

        builder.Property(x => x.DeliveryMethodName)
            .HasMaxLength(Order.MaxDeliveryMethodNameLength)
            .IsRequired();

        builder.Property(x => x.DeliveryPrice)
            .HasPrecision(18, 2);

        // Stored as text so a reordered enum can never silently repoint existing rows.
        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.PaymentIntentId)
            .HasMaxLength(Order.MaxPaymentIntentIdLength);

        // Subtotal and Total are derived from the lines; keep them out of the table
        // so there is exactly one source of truth.
        builder.Ignore(x => x.Subtotal);
        builder.Ignore(x => x.Total);

        builder.OwnsOne(x => x.ShipToAddress, address =>
        {
            address.Property(a => a.RecipientFirstName)
                .HasColumnName("ShipToFirstName")
                .HasMaxLength(ShippingAddress.MaxNameLength)
                .IsRequired();

            address.Property(a => a.RecipientLastName)
                .HasColumnName("ShipToLastName")
                .HasMaxLength(ShippingAddress.MaxNameLength)
                .IsRequired();

            address.Property(a => a.PhoneNumber)
                .HasColumnName("ShipToPhoneNumber")
                .HasMaxLength(ShippingAddress.MaxPhoneLength)
                .IsRequired();

            address.Property(a => a.Country)
                .HasColumnName("ShipToCountry")
                .HasMaxLength(ShippingAddress.MaxCountryLength)
                .IsRequired();

            address.Property(a => a.City)
                .HasColumnName("ShipToCity")
                .HasMaxLength(ShippingAddress.MaxCityLength)
                .IsRequired();

            address.Property(a => a.Street)
                .HasColumnName("ShipToStreet")
                .HasMaxLength(ShippingAddress.MaxStreetLength)
                .IsRequired();

            address.Property(a => a.PostalCode)
                .HasColumnName("ShipToPostalCode")
                .HasMaxLength(ShippingAddress.MaxPostalCodeLength)
                .IsRequired();
        });

        builder.Navigation(x => x.ShipToAddress).IsRequired();

        builder.HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey(item => item.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Items)
            .HasField("_items")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        // The payment provider owns this id; a duplicate means a webhook was
        // reconciled against the wrong order, so let the database reject it.
        builder.HasIndex(x => x.PaymentIntentId)
            .IsUnique()
            .HasFilter("\"PaymentIntentId\" IS NOT NULL");

        builder.HasIndex(x => new { x.BuyerId, x.OrderDate });
    }
}
