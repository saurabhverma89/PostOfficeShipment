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

    public async Task<IReadOnlyList<PostOffice>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.PostOffices
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(PostOffice postOffice, CancellationToken cancellationToken = default)
    {
        await _context.PostOffices.AddAsync(
            postOffice,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(PostOffice postOffice, CancellationToken cancellationToken = default)
    {
        _context.PostOffices.Update(postOffice);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(PostOffice postOffice, CancellationToken cancellationToken = default)
    {
        _context.PostOffices.Remove(postOffice);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsByZipCodeAsync(string zipCode, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.PostOffices
            .Where(x => x.ZipCode == zipCode);

        if (excludeId.HasValue)
        {
            query = query.Where(x => x.Id != excludeId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

}
