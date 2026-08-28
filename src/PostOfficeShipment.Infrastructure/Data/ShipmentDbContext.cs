using Microsoft.EntityFrameworkCore;
using PostOfficeShipment.Domain.Entities;

namespace PostOfficeShipment.Infrastructure.Data;

public class ShipmentDbContext : DbContext
{
    public ShipmentDbContext(DbContextOptions<ShipmentDbContext> options)
        : base(options)
    {
    }

    public DbSet<Shipment> Shipments => Set<Shipment>();

    public DbSet<Package> Packages => Set<Package>();

    public DbSet<Letter> Letters => Set<Letter>();

    public DbSet<PostOffice> PostOffices => Set<PostOffice>();

    public DbSet<ShipmentStatusHistory> ShipmentStatusHistories => Set<ShipmentStatusHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShipmentDbContext).Assembly);
    }
}