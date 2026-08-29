using PostOfficeShipment.Application.DTOs.PostOffices;

namespace PostOfficeShipment.Application.Interfaces;

public interface IPostOfficeService
{
    Task<IReadOnlyList<PostOfficeResponse>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<PostOfficeResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<PostOfficeResponse> CreateAsync(CreatePostOfficeRequest request, CancellationToken cancellationToken = default);

    Task<PostOfficeResponse?> UpdateAsync(int id, UpdatePostOfficeRequest request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

}
