using PostOfficeShipment.Domain.Enums;

namespace PostOfficeShipment.Application.DTOs.Shipments;

public class ShipmentStatusHistoryResponse
{
    public ShipmentStatus Status { get; set; }

    public int PostOfficeId { get; set; }

    public DateTime ChangedAt { get; set; }
}