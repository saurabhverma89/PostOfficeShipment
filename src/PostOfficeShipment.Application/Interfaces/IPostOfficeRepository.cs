using PostOfficeShipment.Domain.Entities;

namespace PostOfficeShipment.Application.Interfaces;

public interface IPostOfficeRepository
{
    Task<PostOffice?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

    Task AddAsync(PostOffice postOffice, CancellationToken cancellationToken = default);

    void Update(PostOffice postOffice);

    void Delete(PostOffice postOffice);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}