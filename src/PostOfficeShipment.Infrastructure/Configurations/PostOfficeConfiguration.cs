using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostOfficeShipment.Domain.Entities;

namespace PostOfficeShipment.Infrastructure.Configurations;

public class PostOfficeConfiguration : IEntityTypeConfiguration<PostOffice>
{
    public void Configure(EntityTypeBuilder<PostOffice> builder)
    {
        builder.ToTable("PostOffices");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ZipCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(x => x.ZipCode)
            .IsUnique();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Address)
            .HasMaxLength(500);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired();

        builder.HasData(
            new PostOffice
            {
                Id = 1,
                ZipCode = "110001",
                Name = "New Delhi Central",
                Address = "Connaught Place, New Delhi",
                CreatedAt = new DateTime(2026, 8, 28, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 8, 28, 0, 0, 0, DateTimeKind.Utc)
            },
            new PostOffice
            {
                Id = 2,
                ZipCode = "201301",
                Name = "Noida Central",
                Address = "Sector 18, Noida",
                CreatedAt = new DateTime(2026, 8, 28, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 8, 28, 0, 0, 0, DateTimeKind.Utc)
            },
            new PostOffice
            {
                Id = 3,
                ZipCode = "122001",
                Name = "Gurugram Central",
                Address = "MG Road, Gurugram",
                CreatedAt = new DateTime(2026, 8, 28, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2026, 8, 28, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}