namespace PostOfficeShipment.Domain.Entities;

public class Package : Shipment
{
    public Package(
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

    protected Package()
    {
    }
}