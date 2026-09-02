using PostOfficeShipment.Domain.Entities;
namespace PostOfficeShipment.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
}
