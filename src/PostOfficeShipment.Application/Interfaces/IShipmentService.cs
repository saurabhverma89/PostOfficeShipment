using PostOfficeShipment.Application.DTOs.Common;
using PostOfficeShipment.Application.DTOs.Shipments;

namespace PostOfficeShipment.Application.Interfaces;

public interface IShipmentService
{
    Task<ShipmentResponse> CreateAsync(CreateShipmentRequest request, CancellationToken cancellationToken = default);

    Task<ShipmentResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<ShipmentResponse?> UpdateAsync(int id, UpdateShipmentRequest request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResponse<ShipmentResponse>> GetPagedAsync(ShipmentQueryRequest request, CancellationToken cancellationToken = default);

}