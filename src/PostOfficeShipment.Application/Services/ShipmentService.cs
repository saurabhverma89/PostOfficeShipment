using PostOfficeShipment.Application.DTOs.Common;
using PostOfficeShipment.Application.DTOs.PostOffices;
using PostOfficeShipment.Application.DTOs.Shipments;
using PostOfficeShipment.Application.Interfaces;
using PostOfficeShipment.Domain.Entities;
using PostOfficeShipment.Domain.Enums;

namespace PostOfficeShipment.Application.Services;

public class ShipmentService : IShipmentService
{
    private readonly IShipmentRepository _shipmentRepository;
    private readonly IPostOfficeRepository _postOfficeRepository;

    public ShipmentService(IShipmentRepository shipmentRepository, IPostOfficeRepository postOfficeRepository)
    {
        _shipmentRepository = shipmentRepository;
        _postOfficeRepository = postOfficeRepository;
    }

    public async Task<ShipmentResponse> CreateAsync(CreateShipmentRequest request, CancellationToken cancellationToken = default)
    {
        // 1. Validate shipment number
        if (string.IsNullOrWhiteSpace(request.ShipmentNumber))
        {
            throw new ArgumentException(
                "Shipment number is required.");
        }

        // 2. Validate weight
        if (request.Weight <= 0)
        {
            throw new ArgumentException(
                "Shipment weight must be greater than zero.");
        }

        // 3. Check shipment number uniqueness
        var shipmentExists =
            await _shipmentRepository.ExistsByShipmentNumberAsync(
                request.ShipmentNumber,
                cancellationToken);

        if (shipmentExists)
        {
            throw new InvalidOperationException(
                $"Shipment number '{request.ShipmentNumber}' already exists.");
        }

        // 4. Validate origin
        var origin =
            await _postOfficeRepository.GetByIdAsync(
                request.OriginPostOfficeId,
                cancellationToken);

        if (origin is null)
        {
            throw new ArgumentException(
                "Origin post office does not exist.");
        }

        // 5. Validate destination
        var destination =
            await _postOfficeRepository.GetByIdAsync(
                request.DestinationPostOfficeId,
                cancellationToken);

        if (destination is null)
        {
            throw new ArgumentException(
                "Destination post office does not exist.");
        }

        // 6. Origin and destination must be different
        if (request.OriginPostOfficeId ==
            request.DestinationPostOfficeId)
        {
            throw new ArgumentException(
                "Origin and destination post offices must be different.");
        }

        // 7. Create the appropriate domain entity
        Shipment shipment = request.Type switch
        {
            ShipmentType.Package => new Package(
                request.ShipmentNumber,
                request.Weight,
                request.OriginPostOfficeId,
                request.DestinationPostOfficeId),

            ShipmentType.Letter => new Letter(
                request.ShipmentNumber,
                request.Weight,
                request.OriginPostOfficeId,
                request.DestinationPostOfficeId),

            _ => throw new ArgumentException(
                "Invalid shipment type.")
        };

        // 8. Add shipment
        await _shipmentRepository.AddAsync(
            shipment,
            cancellationToken);

        // 9. Create initial status history
        var history = new ShipmentStatusHistory
        {
            Shipment = shipment,
            Status = ShipmentStatus.ReceivedAtOrigin,
            PostOfficeId = request.OriginPostOfficeId,
            ChangedAt = shipment.CreatedAt
        };

        await _shipmentRepository.AddStatusHistoryAsync(
            history,
            cancellationToken);

        // 10. Save everything
        await _shipmentRepository.SaveChangesAsync(
            cancellationToken);

        return MapToResponse(shipment);
    }

    public async Task<ShipmentResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var shipment = await _shipmentRepository.GetByIdAsync(id, cancellationToken);

        return shipment is null
            ? null
            : MapToResponse(shipment);
    }

    public async Task<ShipmentResponse?> UpdateAsync(int id, UpdateShipmentRequest request, CancellationToken cancellationToken = default)
    {
        var shipment = await _shipmentRepository.GetByIdAsync(
        id,
        cancellationToken);

        if (shipment is null)
        {
            return null;
        }

        if (request.Weight <= 0)
        {
            throw new ArgumentException("Weight must be greater than zero.");
        }

        var destination = await _postOfficeRepository.GetByIdAsync(
            request.DestinationPostOfficeId,
            cancellationToken);

        if (destination is null)
        {
            throw new ArgumentException("Destination post office does not exist.");
        }

        if (shipment.Status == ShipmentStatus.Delivered)
        {
            throw new InvalidOperationException("A delivered shipment cannot be updated.");
        }

        shipment.Weight = request.Weight;
        shipment.DestinationPostOfficeId = request.DestinationPostOfficeId;

        shipment.DestinationPostOffice = destination;
        shipment.UpdatedAt = DateTime.UtcNow;

        await _shipmentRepository.UpdateAsync(
            shipment,
            cancellationToken);

        return MapToResponse(shipment);

    }


    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var shipment = await _shipmentRepository.GetByIdAsync(id, cancellationToken);

        if (shipment is null)
        {
            return false;
        }

        await _shipmentRepository.DeleteAsync(shipment, cancellationToken);

        return true;

    }

    public async Task<PagedResponse<ShipmentResponse>> GetPagedAsync(ShipmentQueryRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Page < 1)
        {
            request.Page = 1;
        }

        if (request.PageSize < 1)
        {
            request.PageSize = 10;
        }

        if (request.PageSize > 100)
        {
            request.PageSize = 100;
        }

        var (items, totalCount) =
            await _shipmentRepository.GetPagedAsync(
                request,
                cancellationToken);

        return new PagedResponse<ShipmentResponse>
        {
            Items = items
                .Select(MapToResponse)
                .ToList(),

            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }


    private static ShipmentResponse MapToResponse(Shipment shipment)
    {
        return new ShipmentResponse
        {
            Id = shipment.Id,
            ShipmentNumber = shipment.ShipmentNumber,
            Type = shipment.GetType().Name,
            Weight = shipment.Weight,
            WeightCategory = shipment.WeightCategory,
            Status = shipment.Status,

            OriginPostOfficeId = shipment.OriginPostOfficeId,
            DestinationPostOfficeId = shipment.DestinationPostOfficeId,
            CurrentPostOfficeId = shipment.CurrentPostOfficeId,

            OriginPostOffice = MapPostOffice(
                shipment.OriginPostOffice),

            DestinationPostOffice = MapPostOffice(
                shipment.DestinationPostOffice),

            CurrentPostOffice = MapPostOffice(
                shipment.CurrentPostOffice),

            StatusHistory = shipment.StatusHistory
                .OrderBy(x => x.ChangedAt)
                .Select(x => new ShipmentStatusHistoryResponse
                {
                    Status = x.Status,
                    PostOfficeId = x.PostOfficeId,
                    ChangedAt = x.ChangedAt
                })
                .ToList(),

            CreatedAt = shipment.CreatedAt,
            UpdatedAt = shipment.UpdatedAt
        };
    }

    private static PostOfficeResponse? MapPostOffice(PostOffice? postOffice)
    {
        if (postOffice is null)
            return null;

        return new PostOfficeResponse
        {
            Id = postOffice.Id,
            ZipCode = postOffice.ZipCode,
            Name = postOffice.Name,
            Address = postOffice.Address
        };
    }

    public async Task<ShipmentResponse?> MoveAsync(int id, MoveShipmentRequest request, CancellationToken cancellationToken = default)
    {
        var shipment = await _shipmentRepository.GetByIdAsync(id, cancellationToken);

        if (shipment is null)
        {
            return null;
        }

        var postOffice = await _postOfficeRepository.GetByIdAsync(request.PostOfficeId, cancellationToken);

        if (postOffice is null)
        {
            throw new ArgumentException("Post office does not exist.");
        }

        shipment.MoveTo(postOffice.Id);

        await _shipmentRepository.UpdateAsync(shipment, cancellationToken);

        return MapToResponse(shipment);
    }

    public async Task<ShipmentResponse?> ReceiveAtDestinationAsync(int id, CancellationToken cancellationToken = default)
    {
        var shipment = await _shipmentRepository.GetByIdAsync(id, cancellationToken);

        if (shipment is null)
        {
            return null;
        }

        if (shipment.CurrentPostOfficeId != shipment.DestinationPostOfficeId)
        {
            throw new InvalidOperationException("Shipment must be at the destination post office.");
        }

        shipment.MarkAsReceivedAtDestination();

        shipment.StatusHistory.Add(
            new ShipmentStatusHistory
            {
                Status = shipment.Status,
                PostOfficeId = shipment.CurrentPostOfficeId,
                ChangedAt = DateTime.UtcNow
            });

        await _shipmentRepository.UpdateAsync(shipment, cancellationToken);

        return MapToResponse(shipment);

    }

    public async Task<ShipmentResponse?> DeliverAsync(int id, CancellationToken cancellationToken = default)
    {
        var shipment = await _shipmentRepository.GetByIdAsync(id, cancellationToken);

        if (shipment is null)
        {
            return null;
        }

        shipment.MarkAsDelivered();

        shipment.StatusHistory.Add(
            new ShipmentStatusHistory
            {
                Status = shipment.Status,
                PostOfficeId = shipment.CurrentPostOfficeId,
                ChangedAt = DateTime.UtcNow
            });

        await _shipmentRepository.UpdateAsync(shipment, cancellationToken);

        return MapToResponse(shipment);

    }


}