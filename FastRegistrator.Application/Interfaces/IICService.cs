using FastRegistrator.Application.DTOs.ICService;

namespace FastRegistrator.Application.Interfaces
{
    public interface IICService
    {
        Task<ICRegistrationResponse> SendData(ICRegistrationData registrationData, CancellationToken cancellationToken);
    }
}
