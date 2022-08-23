using FastRegistrator.ApplicationCore.DTOs.ICRegistrationDTOs;
using FastRegistrator.ApplicationCore.Interfaces;
using System.Net;
using System.Text.Json;

namespace FastRegistrator.Infrastructure.Services
{
    public class ICService : IICService
    {
        private readonly HttpClient _httpClient;
        private readonly ITestIC _testIC;
        private const string IC_PATH = "/integrationservice/fastregistration/updateuserdata";

        public ICService(HttpClient httpClient, ITestIC testIC)
        {
            _httpClient = httpClient;
            _testIC = testIC;
        }

        public async Task<ICRegistrationResponse> SendData(ICRegistrationData registrationData, CancellationToken cancellationToken)
        {
            var result = await _testIC.SendDataAsync(registrationData, cancellationToken);
            var content = await result.Content.ReadAsStringAsync(cancellationToken);

            var icRegistrationResponse = new ICRegistrationResponse();

            if (!result.IsSuccessStatusCode)
            {
                icRegistrationResponse.ErrorMessage = content;
            }

            icRegistrationResponse.StatusCode = (int)result.StatusCode;

            return await Task.FromResult(icRegistrationResponse);
        }
    }
}
