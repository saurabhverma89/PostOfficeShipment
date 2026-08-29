using PostOfficeShipment.Domain.Entities;

namespace PostOfficeShipment.Application.Interfaces;

public interface IPostOfficeRepository
{
    Task<PostOffice?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PostOffice>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(PostOffice postOffice, CancellationToken cancellationToken = default);

    Task UpdateAsync(PostOffice postOffice, CancellationToken cancellationToken = default);

    Task DeleteAsync(PostOffice postOffice, CancellationToken cancellationToken = default);

    Task<bool> ExistsByZipCodeAsync(string zipCode, int? excludeId = null, CancellationToken cancellationToken = default);

}
