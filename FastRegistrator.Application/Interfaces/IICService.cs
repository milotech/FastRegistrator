using FastRegistrator.ApplicationCore.DTOs.ICRegistrationDTOs;
using FastRegistrator.ApplicationCore.DTOs.RegistrationDTOs;

namespace FastRegistrator.ApplicationCore.Interfaces
{
    public interface IICService
    {
        Task<ICRegistrationResponse> SendData(ICRegistrationData registrationData, CancellationToken cancellationToken);
    }
}
