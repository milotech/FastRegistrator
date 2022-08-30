using FastRegistrator.ApplicationCore.DTOs.ICService;

namespace FastRegistrator.ApplicationCore.Interfaces
{
    public interface IICService
    {
        Task<ICRegistrationResponse> SendData(ICRegistrationData registrationData, CancellationToken cancellationToken);
    }
}
