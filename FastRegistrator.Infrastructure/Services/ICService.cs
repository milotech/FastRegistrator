using FastRegistrator.Application.DTOs.ICService;
using FastRegistrator.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FastRegistrator.Infrastructure.Services
{
    public class ICService : IICService
    {
        private static JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly HttpClient _httpClient;
        private const string IC_PATH = "fastregistration/updateuserdata";

        public ICService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ICRegistrationResponse> SendData(ICRegistrationData registrationData, CancellationToken cancellationToken)
        {
            var stringRegistrationData = JsonSerializer.Serialize(registrationData, _jsonOptions);
            var stringContent = new StringContent(stringRegistrationData, System.Text.Encoding.UTF8, "application/json");

            var result = await _httpClient.PostAsync(IC_PATH, stringContent, cancellationToken);
            var content = await result.Content.ReadAsStringAsync(cancellationToken);

            var icRegistrationResponse = new ICRegistrationResponse();

            if (!result.IsSuccessStatusCode)
            {
                if (string.IsNullOrEmpty(content))
                {
                    result.EnsureSuccessStatusCode();
                }

                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(content, _jsonOptions);
                var icRegistrationError = new ICRegistrationError(problemDetails!.Title, problemDetails!.Detail);
                icRegistrationResponse.ICRegistrationError = icRegistrationError;
            }

            icRegistrationResponse.HttpStatusCode = (int)result.StatusCode;

            return icRegistrationResponse;
        }
    }
}
