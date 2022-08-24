using FastRegistrator.ApplicationCore.DTOs.ICRegistrationDTOs;
using FastRegistrator.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FastRegistrator.Infrastructure.Services
{
    public class ICService : IICService
    {
        private readonly HttpClient _httpClient;
        private const string IC_PATH = "/integrationservice/fastregistration/updateuserdata";

        public ICService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ICRegistrationResponse> SendData(ICRegistrationData registrationData, CancellationToken cancellationToken)
        {
            var stringRegistrationData = JsonSerializer.Serialize(registrationData);
            var stringContent = new StringContent(stringRegistrationData);

            var result = await _httpClient.PostAsync(IC_PATH, stringContent, cancellationToken);
            var content = await result.Content.ReadAsStringAsync(cancellationToken);

            var icRegistrationResponse = new ICRegistrationResponse();

            if (!result.IsSuccessStatusCode)
            {
                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(content);
                var icRegistrationError = new ICRegistrationError(problemDetails!.Title, problemDetails!.Detail);
                icRegistrationResponse.ICRegistrationError = icRegistrationError;
            }

            icRegistrationResponse.HttpStatusCode = (int)result.StatusCode;

            return icRegistrationResponse;
        }
    }
}
