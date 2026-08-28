using PostOfficeShipment.Domain.Enums;

namespace PostOfficeShipment.Domain.Entities;

public class ShipmentStatusHistory
{
    public int Id { get; set; }

    public int ShipmentId { get; set; }

    public ShipmentStatus Status { get; set; }

    public int PostOfficeId { get; set; }

    public DateTime ChangedAt { get; set; }

    public Shipment? Shipment { get; set; }

    public PostOffice? PostOffice { get; set; }
}