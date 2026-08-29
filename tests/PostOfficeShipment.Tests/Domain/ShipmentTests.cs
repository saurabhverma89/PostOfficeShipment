using PostOfficeShipment.Domain.Entities;
using PostOfficeShipment.Domain.Enums;

namespace PostOfficeShipment.Tests.Domain;

public class ShipmentTests
{
    [Fact]
    public void Weight_LessThanOneKg_ShouldReturnLessThanOneKg()
    {
        var shipment = new Package("PKG-001", 0.5m, 1, 2);

        Assert.Equal(
            WeightCategory.LessThan1Kg,
            shipment.WeightCategory);
    }

    [Fact]
    public void Weight_BetweenOneAndFiveKg_ShouldReturnBetweenOneAndFiveKg()
    {
        var shipment = new Package("PKG-002", 3m, 1, 2);

        Assert.Equal(
            WeightCategory.Between1And5Kg,
            shipment.WeightCategory);
    }

    [Fact]
    public void Weight_MoreThanFiveKg_ShouldReturnMoreThanFiveKg()
    {
        var shipment = new Package("PKG-003", 6m, 1, 2);

        Assert.Equal(
            WeightCategory.MoreThan5Kg,
            shipment.WeightCategory);
    }

    [Fact]
    public void MoveTo_ShouldUpdateCurrentPostOffice()
    {
        var shipment = new Package("PKG-004", 2m, 1, 2);

        shipment.MoveTo(3);

        Assert.Equal(3, shipment.CurrentPostOfficeId);

    }

    [Fact]
    public void MoveTo_WhenShipmentIsDelivered_ShouldThrow()
    {
        var shipment = new Package("PKG-005", 2m, 1, 2);
        shipment.MarkAsReceivedAtDestination();
        shipment.MarkAsDelivered();
        Assert.Throws<InvalidOperationException>(() => shipment.MoveTo(3));
    }

    [Fact]
    public void MarkAsReceivedAtDestination_ShouldChangeStatus()
    {
        var shipment = new Package("PKG-006", 2m, 1, 2);

        shipment.MarkAsReceivedAtDestination();

        Assert.Equal(ShipmentStatus.ReceivedAtDestination, shipment.Status);

    }

    [Fact]
    public void MarkAsReceivedAtDestination_WhenNotAtOriginStatus_ShouldThrow()
    {
        var shipment = new Package("PKG-007", 2m, 1, 2);
        shipment.MoveTo(2);
        shipment.MarkAsReceivedAtDestination();

        Assert.Throws<InvalidOperationException>(() => shipment.MarkAsReceivedAtDestination());
    }

    [Fact]
    public void MarkAsDelivered_ShouldChangeStatus()
    {
        var shipment = new Package("PKG-008", 2m, 1, 2);
        shipment.MoveTo(2);
        shipment.MarkAsReceivedAtDestination();
        shipment.MarkAsDelivered();

        Assert.Equal(ShipmentStatus.Delivered, shipment.Status);
    }

    [Fact]
    public void MarkAsDelivered_WhenShipmentIsNotAtDestination_ShouldThrow()
    {
        var shipment = new Package("PKG-009", 2m, 1, 2);
        Assert.Throws<InvalidOperationException>(() => shipment.MarkAsDelivered());
    }

    [Fact]
    public void Weight_ExactlyOneKg_ShouldReturnBetweenOneAndFiveKg()
    {
        var shipment = new Package("PKG-010", 1m, 1, 2);
        Assert.Equal(WeightCategory.Between1And5Kg, shipment.WeightCategory);
    }

    [Fact]
    public void Weight_ExactlyFiveKg_ShouldReturnBetweenOneAndFiveKg()
    {
        var shipment = new Package("PKG-011", 5m, 1, 2);

        Assert.Equal(WeightCategory.Between1And5Kg, shipment.WeightCategory);
    }

    [Fact]
    public void Weight_Zero_ShouldThrow()
    {
        Assert.Throws<ArgumentException>(() => new Package("PKG-012", 0m, 1, 2));
    }

    [Fact]
    public void Weight_Negative_ShouldThrow()
    {
        Assert.Throws<ArgumentException>(() => new Package("PKG-013", -1m, 1, 2));
    }

    [Fact]
    public void DestinationPostOffice_Invalid_ShouldThrow()
    {
        Assert.Throws<ArgumentException>(() => new Package("PKG-014", 2m, 1, 0));
    }

    [Fact]
    public void OriginAndDestinationSame_ShouldThrow()
    {
        Assert.Throws<ArgumentException>(() => new Package("PKG-015", 2m, 1, 1));
    }

    [Fact]
    public void MarkAsReceivedAtDestination_WhenDelivered_ShouldThrow()
    {
        var shipment = new Package("PKG-016", 2m, 1, 2);

        shipment.MoveTo(2);
        shipment.MarkAsReceivedAtDestination();
        shipment.MarkAsDelivered();

        Assert.Throws<InvalidOperationException>(() => shipment.MarkAsReceivedAtDestination());
    }

}
