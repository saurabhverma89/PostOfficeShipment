using PostOfficeShipment.Domain.Entities;

namespace PostOfficeShipment.Application.Interfaces;

public interface IShipmentRepository
{
    Task<Shipment?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> ExistsByShipmentNumberAsync(string shipmentNumber, CancellationToken cancellationToken = default);

    Task AddAsync(Shipment shipment, CancellationToken cancellationToken = default);

    void Update(Shipment shipment);

    void Delete(Shipment shipment);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    Task AddStatusHistoryAsync(ShipmentStatusHistory history, CancellationToken cancellationToken = default);
}