namespace PostOfficeShipment.Domain.Entities;

public class PostOffice
{
    public int Id { get; set; }

    public required string ZipCode { get; set; }

    public required string Name { get; set; }

    public string? Address { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public ICollection<Shipment> OriginShipments { get; set; } = new List<Shipment>();

    public ICollection<Shipment> DestinationShipments { get; set; } = new List<Shipment>();

    public ICollection<Shipment> CurrentShipments { get; set; } = new List<Shipment>();

    public ICollection<ShipmentStatusHistory> ShipmentStatusHistories { get; set; }
        = new List<ShipmentStatusHistory>();
}