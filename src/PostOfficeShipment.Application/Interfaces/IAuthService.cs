using PostOfficeShipment.Application.DTOs.Auth;

namespace PostOfficeShipment.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}