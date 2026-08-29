using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostOfficeShipment.Domain.Entities;

namespace PostOfficeShipment.Infrastructure.Configurations;

public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
{
    public void Configure(EntityTypeBuilder<Shipment> builder)
    {
        builder.ToTable("Shipments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ShipmentNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.ShipmentNumber)
            .IsUnique();

        builder.Property(x => x.Weight)
            .HasPrecision(10, 3);

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired();

        builder.Ignore(x => x.WeightCategory);

        // TPH inheritance
        builder.HasDiscriminator<string>("ShipmentType")
            .HasValue<Package>("Package")
            .HasValue<Letter>("Letter");

        // Origin
        builder.HasOne(x => x.OriginPostOffice)
            .WithMany(x => x.OriginShipments)
            .HasForeignKey(x => x.OriginPostOfficeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Destination
        builder.HasOne(x => x.DestinationPostOffice)
            .WithMany(x => x.DestinationShipments)
            .HasForeignKey(x => x.DestinationPostOfficeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Current location
        builder.HasOne(x => x.CurrentPostOffice)
            .WithMany(x => x.CurrentShipments)
            .HasForeignKey(x => x.CurrentPostOfficeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.StatusHistory)
            .WithOne(x => x.Shipment)
            .HasForeignKey(x => x.ShipmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}