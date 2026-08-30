using System.ComponentModel.DataAnnotations;

namespace PostOfficeShipment.Application.DTOs.Shipments;

public class CreateShipmentRequest
{
    [Required]
    public required string ShipmentNumber { get; set; }

    [Required]
    public required ShipmentType Type { get; set; }

    [Range(0.001, double.MaxValue)]
    public decimal Weight { get; set; }

    [Range(1, int.MaxValue)]
    public int OriginPostOfficeId { get; set; }

    [Range(1, int.MaxValue)]
    public int DestinationPostOfficeId { get; set; }
}