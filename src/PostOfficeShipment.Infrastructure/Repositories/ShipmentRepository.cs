using Microsoft.EntityFrameworkCore;
using PostOfficeShipment.Application.Interfaces;
using PostOfficeShipment.Domain.Entities;
using PostOfficeShipment.Infrastructure.Data;

namespace PostOfficeShipment.Infrastructure.Repositories;

public class ShipmentRepository : IShipmentRepository
{
    private readonly ShipmentDbContext _context;

    public ShipmentRepository(ShipmentDbContext context)
    {
        _context = context;
    }

    public async Task<Shipment?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Shipments
            .Include(x => x.StatusHistory)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByShipmentNumberAsync(string shipmentNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Shipments
            .AnyAsync(
                x => x.ShipmentNumber == shipmentNumber,
                cancellationToken);
    }

    public async Task AddAsync(Shipment shipment, CancellationToken cancellationToken = default)
    {
        await _context.Shipments.AddAsync(
            shipment,
            cancellationToken);
    }

    public void Update(Shipment shipment)
    {
        _context.Shipments.Update(shipment);
    }

    public void Delete(Shipment shipment)
    {
        _context.Shipments.Remove(shipment);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddStatusHistoryAsync(ShipmentStatusHistory history, CancellationToken cancellationToken = default)
    {
        await _context.ShipmentStatusHistories.AddAsync(
            history,
            cancellationToken);
    }
}