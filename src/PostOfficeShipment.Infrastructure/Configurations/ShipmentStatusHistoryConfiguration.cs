using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostOfficeShipment.Domain.Entities;

namespace PostOfficeShipment.Infrastructure.Configurations;

public class ShipmentStatusHistoryConfiguration
    : IEntityTypeConfiguration<ShipmentStatusHistory>
{
    public void Configure(
        EntityTypeBuilder<ShipmentStatusHistory> builder)
    {
        builder.ToTable("ShipmentStatusHistory");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.ChangedAt)
            .IsRequired();

        builder.HasOne(x => x.Shipment)
            .WithMany(x => x.StatusHistory)
            .HasForeignKey(x => x.ShipmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.PostOffice)
            .WithMany(x => x.ShipmentStatusHistories)
            .HasForeignKey(x => x.PostOfficeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new
        {
            x.ShipmentId,
            x.ChangedAt
        });
    }
}