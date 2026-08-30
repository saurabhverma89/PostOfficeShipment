using System.ComponentModel.DataAnnotations;

namespace PostOfficeShipment.Application.DTOs.Shipments;

public class UpdateShipmentRequest
{
    [Range(0.001, double.MaxValue)]
    public decimal Weight { get; set; }

    [Required]
    public int DestinationPostOfficeId { get; set; }
}