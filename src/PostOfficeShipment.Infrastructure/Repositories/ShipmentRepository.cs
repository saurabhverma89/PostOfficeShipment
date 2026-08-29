using Microsoft.EntityFrameworkCore;
using PostOfficeShipment.Application.DTOs.Shipments;
using PostOfficeShipment.Application.Interfaces;
using PostOfficeShipment.Domain.Entities;
using PostOfficeShipment.Domain.Enums;
using PostOfficeShipment.Infrastructure.Data;

namespace PostOfficeShipment.Infrastructure.Repositories;

public class ShipmentRepository : IShipmentRepository
{
    private readonly ShipmentDbContext _context;

    public ShipmentRepository(ShipmentDbContext context)
    {
        _context = context;
    }

    public async Task<Shipment?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Shipments
        .Include(x => x.OriginPostOffice)
        .Include(x => x.DestinationPostOffice)
        .Include(x => x.CurrentPostOffice)
        .Include(x => x.StatusHistory)
        .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByShipmentNumberAsync(string shipmentNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Shipments
            .AnyAsync(
                x => x.ShipmentNumber == shipmentNumber,
                cancellationToken);
    }

    public async Task AddAsync(Shipment shipment, CancellationToken cancellationToken = default)
    {
        await _context.Shipments.AddAsync(
            shipment,
            cancellationToken);
    }

    public void Update(Shipment shipment)
    {
        _context.Shipments.Update(shipment);
    }

    public void Delete(Shipment shipment)
    {
        _context.Shipments.Remove(shipment);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddStatusHistoryAsync(ShipmentStatusHistory history, CancellationToken cancellationToken = default)
    {
        await _context.ShipmentStatusHistories.AddAsync(
            history,
            cancellationToken);
    }

    public async Task<(IReadOnlyList<Shipment> Items, int TotalCount)> GetPagedAsync(ShipmentQueryRequest request, CancellationToken cancellationToken = default)
    {
        IQueryable<Shipment> query = _context.Shipments
        .Include(x => x.OriginPostOffice)
        .Include(x => x.DestinationPostOffice)
        .Include(x => x.CurrentPostOffice)
        .Include(x => x.StatusHistory);

        if (!string.IsNullOrWhiteSpace(request.ShipmentNumber))
        {
            var shipmentNumber = request.ShipmentNumber.Trim();

            query = query.Where(x =>
                x.ShipmentNumber == shipmentNumber);
        }

        if (request.Status.HasValue)
        {
            query = query.Where(x =>
                x.Status == request.Status.Value);
        }

        if (request.PostOfficeId.HasValue)
        {
            var postOfficeId = request.PostOfficeId.Value;

            query = query.Where(x =>
                x.CurrentPostOfficeId == postOfficeId);
        }

        if (request.WeightCategory.HasValue)
        {
            query = request.WeightCategory.Value switch
            {
                WeightCategory.LessThan1Kg =>
                    query.Where(x => x.Weight < 1),

                WeightCategory.Between1And5Kg =>
                    query.Where(x => x.Weight >= 1 && x.Weight <= 5),

                WeightCategory.MoreThan5Kg =>
                    query.Where(x => x.Weight > 5),

                _ => query
            };
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);

    }

}