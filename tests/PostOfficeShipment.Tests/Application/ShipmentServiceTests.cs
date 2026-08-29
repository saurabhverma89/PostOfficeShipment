using Moq;
using PostOfficeShipment.Application.DTOs.Shipments;
using PostOfficeShipment.Application.Interfaces;
using PostOfficeShipment.Application.Services;
using PostOfficeShipment.Domain.Entities;
using PostOfficeShipment.Domain.Enums;

namespace PostOfficeShipment.Tests.Application;

public class ShipmentServiceTests
{
    private readonly Mock<IShipmentRepository> _shipmentRepositoryMock;
    private readonly Mock<IPostOfficeRepository> _postOfficeRepositoryMock;
    private readonly ShipmentService _service;

    public ShipmentServiceTests()
    {
        _shipmentRepositoryMock = new Mock<IShipmentRepository>();
        _postOfficeRepositoryMock = new Mock<IPostOfficeRepository>();

        _service = new ShipmentService(
            _shipmentRepositoryMock.Object,
            _postOfficeRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_WithValidPackage_ShouldCreateShipment()
    {
        // Arrange
        var origin = new PostOffice
        {
            Id = 1,
            ZipCode = "110001",
            Name = "New Delhi Central"
        };

        var destination = new PostOffice
        {
            Id = 2,
            ZipCode = "201301",
            Name = "Noida Central"
        };

        var request = new CreateShipmentRequest
        {
            ShipmentNumber = "PKG-001",
            Type = ShipmentType.Package,
            Weight = 2m,
            OriginPostOfficeId = 1,
            DestinationPostOfficeId = 2
        };

        _shipmentRepositoryMock
            .Setup(x => x.ExistsByShipmentNumberAsync(
                request.ShipmentNumber,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _postOfficeRepositoryMock
            .Setup(x => x.GetByIdAsync(
                1,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(origin);

        _postOfficeRepositoryMock
            .Setup(x => x.GetByIdAsync(
                2,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(destination);

        // Act
        var result = await _service.CreateAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("PKG-001", result.ShipmentNumber);
        Assert.Equal("Package", result.Type);
        Assert.Equal(2m, result.Weight);
        Assert.Equal(
            ShipmentStatus.ReceivedAtOrigin,
            result.Status);

        Assert.Equal(1, result.OriginPostOfficeId);
        Assert.Equal(2, result.DestinationPostOfficeId);
        Assert.Equal(1, result.CurrentPostOfficeId);

        _shipmentRepositoryMock.Verify(
            x => x.AddAsync(
                It.Is<Package>(p =>
                    p.ShipmentNumber == "PKG-001" &&
                    p.Weight == 2m &&
                    p.OriginPostOfficeId == 1 &&
                    p.DestinationPostOfficeId == 2),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _shipmentRepositoryMock.Verify(
            x => x.AddStatusHistoryAsync(
                It.Is<ShipmentStatusHistory>(h =>
                    h.Status == ShipmentStatus.ReceivedAtOrigin &&
                    h.PostOfficeId == 1),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _shipmentRepositoryMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }


    [Fact]
    public async Task CreateAsync_WhenShipmentNumberAlreadyExists_ShouldThrow()
    {
        // Arrange
        var request = new CreateShipmentRequest
        {
            ShipmentNumber = "PKG-002",
            Type = ShipmentType.Package,
            Weight = 2m,
            OriginPostOfficeId = 1,
            DestinationPostOfficeId = 2
        };

        _shipmentRepositoryMock
            .Setup(x => x.ExistsByShipmentNumberAsync(
                request.ShipmentNumber,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateAsync(request));

        Assert.Equal(
            "Shipment number 'PKG-002' already exists.",
            exception.Message);

        _shipmentRepositoryMock.Verify(
            x => x.AddAsync(
                It.IsAny<Shipment>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        _shipmentRepositoryMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);

    }


    [Fact]
    public async Task CreateAsync_WhenOriginDoesNotExist_ShouldThrow()
    {
        // Arrange
        var request = new CreateShipmentRequest
        {
            ShipmentNumber = "PKG-003",
            Type = ShipmentType.Package,
            Weight = 2m,
            OriginPostOfficeId = 999,
            DestinationPostOfficeId = 2
        };

        _shipmentRepositoryMock
            .Setup(x => x.ExistsByShipmentNumberAsync(
                request.ShipmentNumber,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _postOfficeRepositoryMock
            .Setup(x => x.GetByIdAsync(
                999,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((PostOffice?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _service.CreateAsync(request));

        Assert.Equal(
            "Origin post office does not exist.",
            exception.Message);

        _shipmentRepositoryMock.Verify(
            x => x.AddAsync(
                It.IsAny<Shipment>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

    }


    [Fact]
    public async Task CreateAsync_WhenDestinationDoesNotExist_ShouldThrow()
    {
        // Arrange
        var origin = new PostOffice
        {
            Id = 1,
            ZipCode = "110001",
            Name = "New Delhi Central"
        };

        var request = new CreateShipmentRequest
        {
            ShipmentNumber = "PKG-004",
            Type = ShipmentType.Package,
            Weight = 2m,
            OriginPostOfficeId = 1,
            DestinationPostOfficeId = 999
        };

        _shipmentRepositoryMock
            .Setup(x => x.ExistsByShipmentNumberAsync(
                request.ShipmentNumber,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _postOfficeRepositoryMock
            .Setup(x => x.GetByIdAsync(
                1,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(origin);

        _postOfficeRepositoryMock
            .Setup(x => x.GetByIdAsync(
                999,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((PostOffice?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _service.CreateAsync(request));

        Assert.Equal(
            "Destination post office does not exist.",
            exception.Message);

        _shipmentRepositoryMock.Verify(
            x => x.AddAsync(
                It.IsAny<Shipment>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

    }


    [Fact]
    public async Task CreateAsync_WhenWeightIsInvalid_ShouldThrow()
    {
        // Arrange
        var request = new CreateShipmentRequest
        {
            ShipmentNumber = "PKG-005",
            Type = ShipmentType.Package,
            Weight = 0m,
            OriginPostOfficeId = 1,
            DestinationPostOfficeId = 2
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _service.CreateAsync(request));

        Assert.Equal(
            "Shipment weight must be greater than zero.",
            exception.Message);

        _shipmentRepositoryMock.Verify(
            x => x.ExistsByShipmentNumberAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WithValidRequest_ShouldUpdateShipment()
    {
        // Arrange
        var shipment = new Package("PKG-100", 2m, 1, 2);

        shipment.Id = 1;

        var destination = new PostOffice
        {
            Id = 3,
            ZipCode = "122001",
            Name = "Gurugram Central"
        };

        var request = new UpdateShipmentRequest
        {
            Weight = 4m,
            DestinationPostOfficeId = 3
        };

        _shipmentRepositoryMock
            .Setup(x => x.GetByIdAsync(
                1,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(shipment);

        _postOfficeRepositoryMock
            .Setup(x => x.GetByIdAsync(
                3,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(destination);

        // Act
        var result = await _service.UpdateAsync(1, request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(4m, result.Weight);
        Assert.Equal(3, result.DestinationPostOfficeId);

        _shipmentRepositoryMock.Verify(
            x => x.UpdateAsync(
                shipment,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenShipmentDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var request = new UpdateShipmentRequest
        {
            Weight = 3m,
            DestinationPostOfficeId = 2
        };

        _shipmentRepositoryMock
            .Setup(x => x.GetByIdAsync(
                999,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Shipment?)null);

        // Act
        var result = await _service.UpdateAsync(999, request);

        // Assert
        Assert.Null(result);

        _shipmentRepositoryMock.Verify(
            x => x.UpdateAsync(
                It.IsAny<Shipment>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

    }

    [Fact]
    public async Task UpdateAsync_WhenShipmentIsDelivered_ShouldThrow()
    {
        // Arrange
        var shipment = new Package(
        "PKG-101",
        2m,
        1,
        2);

        shipment.Id = 1;

        shipment.MoveTo(2);
        shipment.MarkAsReceivedAtDestination();
        shipment.MarkAsDelivered();

        var request = new UpdateShipmentRequest
        {
            Weight = 3m,
            DestinationPostOfficeId = 2
        };

        var destination = new PostOffice
        {
            Id = 2,
            ZipCode = "201301",
            Name = "Noida Central"
        };

        _shipmentRepositoryMock
            .Setup(x => x.GetByIdAsync(
                1,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(shipment);

        _postOfficeRepositoryMock
            .Setup(x => x.GetByIdAsync(
                2,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(destination);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.UpdateAsync(1, request));

        _shipmentRepositoryMock.Verify(
            x => x.UpdateAsync(
                It.IsAny<Shipment>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

    }

    [Fact]
    public async Task DeleteAsync_WhenShipmentExists_ShouldDeleteShipment()
    {
        // Arrange
        var shipment = new Package(
        "PKG-102",
        2m,
        1,
        2);

        shipment.Id = 1;

        _shipmentRepositoryMock
            .Setup(x => x.GetByIdAsync(
                1,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(shipment);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        Assert.True(result);

        _shipmentRepositoryMock.Verify(
            x => x.DeleteAsync(
                shipment,
                It.IsAny<CancellationToken>()),
            Times.Once);

    }

    [Fact]
    public async Task DeleteAsync_WhenShipmentDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        _shipmentRepositoryMock
        .Setup(x => x.GetByIdAsync(
        999,
        It.IsAny<CancellationToken>()))
        .ReturnsAsync((Shipment?)null);

        // Act
        var result = await _service.DeleteAsync(999);

        // Assert
        Assert.False(result);

        _shipmentRepositoryMock.Verify(
            x => x.DeleteAsync(
                It.IsAny<Shipment>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task MoveAsync_WithValidPostOffice_ShouldMoveShipment()
    {
        // Arrange
        var shipment = new Package(
        "PKG-103",
        2m,
        1,
        2);

        shipment.Id = 1;

        var postOffice = new PostOffice
        {
            Id = 3,
            ZipCode = "122001",
            Name = "Gurugram Central"
        };

        var request = new MoveShipmentRequest
        {
            PostOfficeId = 3
        };

        _shipmentRepositoryMock
            .Setup(x => x.GetByIdAsync(
                1,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(shipment);

        _postOfficeRepositoryMock
            .Setup(x => x.GetByIdAsync(
                3,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(postOffice);

        // Act
        var result = await _service.MoveAsync(1, request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.CurrentPostOfficeId);

        _shipmentRepositoryMock.Verify(
            x => x.UpdateAsync(
                shipment,
                It.IsAny<CancellationToken>()),
            Times.Once);

    }

    [Fact]
    public async Task MoveAsync_WhenPostOfficeDoesNotExist_ShouldThrow()
    {
        // Arrange
        var shipment = new Package(
        "PKG-104",
        2m,
        1,
        2);

        shipment.Id = 1;

        var request = new MoveShipmentRequest
        {
            PostOfficeId = 999
        };

        _shipmentRepositoryMock
            .Setup(x => x.GetByIdAsync(
                1,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(shipment);

        _postOfficeRepositoryMock
            .Setup(x => x.GetByIdAsync(
                999,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((PostOffice?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.MoveAsync(1, request));

        _shipmentRepositoryMock.Verify(
            x => x.UpdateAsync(
                It.IsAny<Shipment>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

    }

    [Fact]
    public async Task ReceiveAtDestinationAsync_WhenAtDestination_ShouldUpdateStatus()
    {
        // Arrange
        var shipment = new Package(
        "PKG-105",
        2m,
        1,
        2);

        shipment.Id = 1;

        shipment.MoveTo(2);

        _shipmentRepositoryMock
            .Setup(x => x.GetByIdAsync(
                1,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(shipment);

        // Act
        var result =
            await _service.ReceiveAtDestinationAsync(1);

        // Assert
        Assert.NotNull(result);

        Assert.Equal(
            ShipmentStatus.ReceivedAtDestination,
            result.Status);

        _shipmentRepositoryMock.Verify(
            x => x.UpdateAsync(
                shipment,
                It.IsAny<CancellationToken>()),
            Times.Once);


    }

    [Fact]
    public async Task ReceiveAtDestinationAsync_WhenNotAtDestination_ShouldThrow()
    {
        // Arrange
        var shipment = new Package(
        "PKG-106",
        2m,
        1,
        2);

        shipment.Id = 1;

        _shipmentRepositoryMock
            .Setup(x => x.GetByIdAsync(
                1,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(shipment);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.ReceiveAtDestinationAsync(1));

        _shipmentRepositoryMock.Verify(
            x => x.UpdateAsync(
                It.IsAny<Shipment>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

    }

    [Fact]
    public async Task DeliverAsync_WhenReceivedAtDestination_ShouldDeliver()
    {
        // Arrange
        var shipment = new Package(
        "PKG-107",
        2m,
        1,
        2);

        shipment.Id = 1;

        shipment.MoveTo(2);
        shipment.MarkAsReceivedAtDestination();

        _shipmentRepositoryMock
            .Setup(x => x.GetByIdAsync(
                1,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(shipment);

        // Act
        var result = await _service.DeliverAsync(1);

        // Assert
        Assert.NotNull(result);

        Assert.Equal(
            ShipmentStatus.Delivered,
            result.Status);

        _shipmentRepositoryMock.Verify(
            x => x.UpdateAsync(
                shipment,
                It.IsAny<CancellationToken>()),
            Times.Once);

    }

    [Fact]
    public async Task DeliverAsync_WhenShipmentDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        _shipmentRepositoryMock
        .Setup(x => x.GetByIdAsync(
        999,
        It.IsAny<CancellationToken>()))
        .ReturnsAsync((Shipment?)null);

        // Act
        var result = await _service.DeliverAsync(999);

        // Assert
        Assert.Null(result);

        _shipmentRepositoryMock.Verify(
            x => x.UpdateAsync(
                It.IsAny<Shipment>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

    }


}
