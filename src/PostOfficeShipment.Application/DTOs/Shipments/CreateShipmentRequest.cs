namespace PostOfficeShipment.Application.DTOs.Shipments;

public class CreateShipmentRequest
{
    public required string ShipmentNumber { get; set; }

    public required ShipmentType Type { get; set; }

    public decimal Weight { get; set; }

    public int OriginPostOfficeId { get; set; }

    public int DestinationPostOfficeId { get; set; }
}