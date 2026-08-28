namespace PostOfficeShipment.Application.DTOs.Shipments;

public class UpdateShipmentRequest
{
    public decimal Weight { get; set; }

    public int DestinationPostOfficeId { get; set; }
}