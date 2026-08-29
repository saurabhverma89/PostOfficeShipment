using PostOfficeShipment.Application.DTOs.PostOffices;
using PostOfficeShipment.Domain.Enums;

namespace PostOfficeShipment.Application.DTOs.Shipments;

public class ShipmentResponse
{
    public int Id { get; set; }

    public string ShipmentNumber { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public decimal Weight { get; set; }

    public WeightCategory WeightCategory { get; set; }

    public ShipmentStatus Status { get; set; }

    public int OriginPostOfficeId { get; set; }

    public int DestinationPostOfficeId { get; set; }

    public int CurrentPostOfficeId { get; set; }

    public PostOfficeResponse? OriginPostOffice { get; set; }

    public PostOfficeResponse? DestinationPostOffice { get; set; }

    public PostOfficeResponse? CurrentPostOffice { get; set; }

    public List<ShipmentStatusHistoryResponse> StatusHistory { get; set; } = new();

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}