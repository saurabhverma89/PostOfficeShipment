namespace PostOfficeShipment.Domain.Entities;

public class Letter : Shipment
{
    public Letter(
        string shipmentNumber,
        decimal weight,
        int originPostOfficeId,
        int destinationPostOfficeId)
        : base(
            shipmentNumber,
            weight,
            originPostOfficeId,
            destinationPostOfficeId)
    {
    }

    protected Letter()
    {
    }
}