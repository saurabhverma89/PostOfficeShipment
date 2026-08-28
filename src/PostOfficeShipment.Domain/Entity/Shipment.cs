using PostOfficeShipment.Domain.Enums;

namespace PostOfficeShipment.Domain.Entities;

public abstract class Shipment
{
    public int Id { get; set; }

    public required string ShipmentNumber { get; set; }

    public decimal Weight { get; set; }

    public ShipmentStatus Status { get; set; }

    public int OriginPostOfficeId { get; set; }

    public int DestinationPostOfficeId { get; set; }

    public int CurrentPostOfficeId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public PostOffice? OriginPostOffice { get; set; }

    public PostOffice? DestinationPostOffice { get; set; }

    public PostOffice? CurrentPostOffice { get; set; }

    public ICollection<ShipmentStatusHistory> StatusHistory { get; set; }
        = new List<ShipmentStatusHistory>();

    public WeightCategory WeightCategory
    {
        get
        {
            if (Weight < 1)
                return WeightCategory.LessThan1Kg;

            if (Weight <= 5)
                return WeightCategory.Between1And5Kg;

            return WeightCategory.MoreThan5Kg;
        }
    }
}