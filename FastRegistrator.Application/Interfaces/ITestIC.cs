using FastRegistrator.ApplicationCore.DTOs.ICRegistrationDTOs;

namespace FastRegistrator.ApplicationCore.Interfaces
{
    public interface ITestIC
    {
        public Task<HttpResponseMessage> SendDataAsync(ICRegistrationData registrationData, CancellationToken cancellationToken);
    }
}
