using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public class DeliveryMethodConfiguration :
    IEntityTypeConfiguration<DeliveryMethod>
{
    public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
    {
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(DeliveryMethod.MaxNameLength);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(DeliveryMethod.MaxDescriptionLength);

        builder.Property(x => x.DeliveryTime)
            .IsRequired()
            .HasMaxLength(DeliveryMethod.MaxDeliveryTimeLength);


        builder.Property(x => x.Price)
            .HasPrecision(18, 2);

        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}
