namespace PostOfficeShipment.Application.DTOs.Shipments;

public class ShipmentSummaryResponse
{
    public int Total { get; set; }

    public int ReceivedAtOrigin { get; set; }

    public int ReceivedAtDestination { get; set; }

    public int Delivered { get; set; }
}