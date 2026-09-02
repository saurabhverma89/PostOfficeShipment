using Microsoft.EntityFrameworkCore;
using PostOfficeShipment.Application.Interfaces;
using PostOfficeShipment.Domain.Entities;
using PostOfficeShipment.Infrastructure.Data;

namespace PostOfficeShipment.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ShipmentDbContext _context;

    public UserRepository(ShipmentDbContext context)
    {
        _context = context;
    }

    public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return _context.Users.SingleOrDefaultAsync(x => x.Username == username, cancellationToken);
    }
}