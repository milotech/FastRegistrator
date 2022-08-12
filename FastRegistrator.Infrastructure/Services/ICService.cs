using FastRegistrator.ApplicationCore.DTOs.ICRegistrationDTOs;
using FastRegistrator.ApplicationCore.DTOs.RegistrationDTOs;
using FastRegistrator.ApplicationCore.Interfaces;

namespace FastRegistrator.Infrastructure.Services
{
    public class ICService : IICService
    {
        public Task<ICRegistrationResponse> SendData(ICRegistrationData registrationData, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
