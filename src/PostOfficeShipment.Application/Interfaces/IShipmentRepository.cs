using PostOfficeShipment.Application.DTOs.Shipments;
using PostOfficeShipment.Domain.Entities;

namespace PostOfficeShipment.Application.Interfaces;

public interface IShipmentRepository
{
    Task<Shipment?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> ExistsByShipmentNumberAsync(string shipmentNumber, CancellationToken cancellationToken = default);

    Task AddAsync(Shipment shipment, CancellationToken cancellationToken = default);

    Task UpdateAsync(Shipment shipment, CancellationToken cancellationToken = default);

    Task DeleteAsync(Shipment shipment, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    Task AddStatusHistoryAsync(ShipmentStatusHistory history, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Shipment> Items, int TotalCount)> GetPagedAsync(ShipmentQueryRequest request, CancellationToken cancellationToken = default);
}