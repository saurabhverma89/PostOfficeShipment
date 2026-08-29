using PostOfficeShipment.Domain.Enums;

namespace PostOfficeShipment.Domain.Entities;

public abstract class Shipment
{
    protected Shipment()
    {
    }

    protected Shipment(
    string shipmentNumber,
    decimal weight,
    int originPostOfficeId,
    int destinationPostOfficeId)
    {
        if (string.IsNullOrWhiteSpace(shipmentNumber))
            throw new ArgumentException(
                "Shipment number is required.",
                nameof(shipmentNumber));

        if (weight <= 0)
            throw new ArgumentException(
                "Shipment weight must be greater than zero.",
                nameof(weight));

        if (originPostOfficeId <= 0)
            throw new ArgumentException(
                "Origin post office is required.",
                nameof(originPostOfficeId));

        if (destinationPostOfficeId <= 0)
            throw new ArgumentException(
                "Destination post office is required.",
                nameof(destinationPostOfficeId));

        if (originPostOfficeId == destinationPostOfficeId)
            throw new ArgumentException(
                "Origin and destination post offices must be different.");

        ShipmentNumber = shipmentNumber;
        Weight = weight;

        OriginPostOfficeId = originPostOfficeId;
        DestinationPostOfficeId = destinationPostOfficeId;
        CurrentPostOfficeId = originPostOfficeId;

        Status = ShipmentStatus.ReceivedAtOrigin;

        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public int Id { get; set; }

    public string ShipmentNumber { get; private set; } = string.Empty;

    public decimal Weight { get; set; }

    public ShipmentStatus Status { get; private set; }

    public int OriginPostOfficeId { get; set; }

    public int DestinationPostOfficeId { get; set; }

    public int CurrentPostOfficeId { get; private set; }

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

    public void MoveTo(int postOfficeId)
    {
        if (Status == ShipmentStatus.Delivered)
        {
            throw new InvalidOperationException("A delivered shipment cannot be moved.");
        }

        if (postOfficeId <= 0)
        {
            throw new ArgumentException("Post office ID must be greater than zero.");
        }

        CurrentPostOfficeId = postOfficeId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsReceivedAtDestination()
    {
        if (Status != ShipmentStatus.ReceivedAtOrigin)
        {
            throw new InvalidOperationException("Shipment must be received at origin before it can be received at destination.");
        }

        Status = ShipmentStatus.ReceivedAtDestination;
        UpdatedAt = DateTime.UtcNow;

    }

    public void MarkAsDelivered()
    {
        if (Status != ShipmentStatus.ReceivedAtDestination)
        {
            throw new InvalidOperationException("Shipment must be received at destination before it can be delivered.");
        }

        Status = ShipmentStatus.Delivered;
        UpdatedAt = DateTime.UtcNow;

    }

    public void ReceiveAtPostOffice(int postOfficeId)
    {
        if (Status == ShipmentStatus.Delivered)
        {
            throw new InvalidOperationException("A delivered shipment cannot be received again.");
        }

        if (postOfficeId <= 0)
        {
            throw new ArgumentException("Post office ID must be greater than zero.");
        }

        if (postOfficeId == OriginPostOfficeId)
        {
            if (Status != ShipmentStatus.ReceivedAtOrigin)
            {
                throw new InvalidOperationException("Shipment cannot be received at origin in its current status.");
            }

            CurrentPostOfficeId = postOfficeId;
            Status = ShipmentStatus.ReceivedAtOrigin;
        }
        else if (postOfficeId == DestinationPostOfficeId)
        {
            if (Status != ShipmentStatus.ReceivedAtOrigin)
            {
                throw new InvalidOperationException("Shipment must be received at origin before reaching destination.");
            }

            CurrentPostOfficeId = postOfficeId;
            Status = ShipmentStatus.ReceivedAtDestination;
        }
        else
        {
            CurrentPostOfficeId = postOfficeId;
        }

        UpdatedAt = DateTime.UtcNow;

    }



    public void UpdateStatus(ShipmentStatus newStatus)
    {
        if (newStatus == Status)
            return;

        if (!IsValidStatusTransition(Status, newStatus))
        {
            throw new InvalidOperationException($"Invalid status transition from {Status} to {newStatus}.");
        }

        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;
    }

    private static bool IsValidStatusTransition(ShipmentStatus currentStatus, ShipmentStatus newStatus)
    {
        return currentStatus switch
        {
            ShipmentStatus.ReceivedAtOrigin =>
                newStatus == ShipmentStatus.ReceivedAtDestination,

            ShipmentStatus.ReceivedAtDestination =>
                newStatus == ShipmentStatus.Delivered,

            ShipmentStatus.Delivered => false,

            _ => false
        };
    }
}