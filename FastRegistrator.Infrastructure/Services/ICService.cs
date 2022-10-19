using FastRegistrator.Application.DTOs.ICService;
using FastRegistrator.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ICService> _logger;
        private const string IC_PATH = "https://test-vtbcb-internal/api/Vtbcb-AuthServices-Test/api/fastregistration/updateuserdata";

        public ICService(HttpClient httpClient, ILogger<ICService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<ICRegistrationResponse> SendData(ICRegistrationData registrationData, CancellationToken cancellationToken)
        {
            var stringContent = new StringContent(registrationData.Data, System.Text.Encoding.UTF8, "application/json");

            _logger.LogInformation(stringContent.ToString());
            _logger.LogInformation("-----------");
            _logger.LogInformation(registrationData.Data);

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
