using System.ComponentModel.DataAnnotations;

namespace PostOfficeShipment.Application.DTOs.Shipments;

public class MoveShipmentRequest
{
    [Required]
    public int PostOfficeId { get; set; }
}
