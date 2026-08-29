using PostOfficeShipment.Domain.Enums;

namespace PostOfficeShipment.Application.DTOs.Shipments;

public class ShipmentQueryRequest
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? ShipmentNumber { get; set; }

    public ShipmentStatus? Status { get; set; }

    public int? PostOfficeId { get; set; }

    public WeightCategory? WeightCategory { get; set; }

}
