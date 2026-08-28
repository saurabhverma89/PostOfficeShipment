using Microsoft.EntityFrameworkCore;
using PostOfficeShipment.Application.Interfaces;
using PostOfficeShipment.Domain.Entities;
using PostOfficeShipment.Infrastructure.Data;

namespace PostOfficeShipment.Infrastructure.Repositories;

public class PostOfficeRepository : IPostOfficeRepository
{
    private readonly ShipmentDbContext _context;

    public PostOfficeRepository(ShipmentDbContext context)
    {
        _context = context;
    }

    public async Task<PostOffice?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.PostOffices
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.PostOffices
            .AnyAsync(x => x.Id == id, cancellationToken);
    }

    public async Task AddAsync(PostOffice postOffice, CancellationToken cancellationToken = default)
    {
        await _context.PostOffices.AddAsync(
            postOffice,
            cancellationToken);
    }

    public void Update(PostOffice postOffice)
    {
        _context.PostOffices.Update(postOffice);
    }

    public void Delete(PostOffice postOffice)
    {
        _context.PostOffices.Remove(postOffice);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}