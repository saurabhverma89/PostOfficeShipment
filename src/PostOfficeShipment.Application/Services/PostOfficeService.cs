using PostOfficeShipment.Application.DTOs.PostOffices;
using PostOfficeShipment.Application.Interfaces;
using PostOfficeShipment.Domain.Entities;

namespace PostOfficeShipment.Application.Services;

public class PostOfficeService : IPostOfficeService
{
    private readonly IPostOfficeRepository _postOfficeRepository;

    public PostOfficeService(IPostOfficeRepository postOfficeRepository)
    {
        _postOfficeRepository = postOfficeRepository;
    }

    public async Task<IReadOnlyList<PostOfficeResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var postOffices = await _postOfficeRepository.GetAllAsync(
                cancellationToken);

        return postOffices
            .Select(MapToResponse)
            .ToList();
    }

    public async Task<PostOfficeResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var postOffice = await _postOfficeRepository.GetByIdAsync(
                id,
                cancellationToken);

        return postOffice is null
            ? null
            : MapToResponse(postOffice);
    }

    public async Task<PostOfficeResponse> CreateAsync(CreatePostOfficeRequest request, CancellationToken cancellationToken = default)
    {
        var zipCode = request.ZipCode.Trim();

        if (string.IsNullOrWhiteSpace(zipCode))
        {
            throw new ArgumentException("ZIP code is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException("Post office name is required.");
        }

        if (await _postOfficeRepository.ExistsByZipCodeAsync(zipCode,
                cancellationToken: cancellationToken))
        {
            throw new InvalidOperationException("A post office with this ZIP code already exists.");
        }

        var now = DateTime.UtcNow;

        var postOffice = new PostOffice
        {
            ZipCode = zipCode,
            Name = request.Name.Trim(),
            Address = request.Address?.Trim(),
            CreatedAt = now,
            UpdatedAt = now
        };

        await _postOfficeRepository.AddAsync(
            postOffice,
            cancellationToken);

        return MapToResponse(postOffice);
    }

    public async Task<PostOfficeResponse?> UpdateAsync(int id, UpdatePostOfficeRequest request, CancellationToken cancellationToken = default)
    {
        var postOffice =
            await _postOfficeRepository.GetByIdAsync(
                id,
                cancellationToken);

        if (postOffice is null)
        {
            return null;
        }

        var zipCode = request.ZipCode.Trim();

        if (string.IsNullOrWhiteSpace(zipCode))
        {
            throw new ArgumentException("ZIP code is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException("Post office name is required.");
        }

        if (await _postOfficeRepository.ExistsByZipCodeAsync(
                zipCode,
                id,
                cancellationToken))
        {
            throw new InvalidOperationException("A post office with this ZIP code already exists.");
        }

        postOffice.ZipCode = zipCode;
        postOffice.Name = request.Name.Trim();
        postOffice.Address = request.Address?.Trim();
        postOffice.UpdatedAt = DateTime.UtcNow;

        await _postOfficeRepository.UpdateAsync(
            postOffice,
            cancellationToken);

        return MapToResponse(postOffice);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var postOffice = await _postOfficeRepository.GetByIdAsync(
                id,
                cancellationToken);

        if (postOffice is null)
        {
            return false;
        }

        await _postOfficeRepository.DeleteAsync(
            postOffice,
            cancellationToken);

        return true;
    }

    private static PostOfficeResponse MapToResponse(PostOffice postOffice)
    {
        return new PostOfficeResponse
        {
            Id = postOffice.Id,
            ZipCode = postOffice.ZipCode,
            Name = postOffice.Name,
            Address = postOffice.Address,
            CreatedAt = postOffice.CreatedAt,
            UpdatedAt = postOffice.UpdatedAt
        };
    }

}
