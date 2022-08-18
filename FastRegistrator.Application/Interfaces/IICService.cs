using FastRegistrator.ApplicationCore.DTOs.ICRegistrationDTOs;

namespace FastRegistrator.ApplicationCore.Interfaces
{
    public interface IICService
    {
        Task<ICRegistrationResponse> SendData(ICRegistrationData registrationData, CancellationToken cancellationToken);
    }
}
