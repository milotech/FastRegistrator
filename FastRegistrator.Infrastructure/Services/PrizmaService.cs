using FastRegistrator.Application.DTOs.PrizmaService;
using FastRegistrator.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FastRegistrator.Infrastructure.Services;

public class PrizmaService : IPrizmaService
{
    private static JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient;
    private readonly ILogger<PrizmaService> _logger;
    private const string PERSON_CHECK_PATH = "PersonCheck";

    public PrizmaService(
        HttpClient httpClient,
        ILogger<PrizmaService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<PersonCheckResponse> PersonCheck(PersonCheckRequest personCheckRequest, CancellationToken cancelToken)
    {
        var requestUri = $"{PERSON_CHECK_PATH}?Fio={personCheckRequest.Fio}&PassportNumber={personCheckRequest.PassportNumber}";
        if (!string.IsNullOrEmpty(personCheckRequest.Inn))
        {
            requestUri += $"&Inn={personCheckRequest.Inn}";
        }
        if (personCheckRequest.DateOfBirth is not null)
        {
            requestUri += $"&DateOfBirth={personCheckRequest.DateOfBirth.Value:yyyy-MM-dd}";
        }

        _logger.LogInformation(requestUri);

        var result = await _httpClient.GetAsync(requestUri, cancelToken);
        var content = await result.Content.ReadAsStringAsync(cancelToken);

        var personCheckResponse = new PersonCheckResponse();        

        if (result.IsSuccessStatusCode)
        {
            _logger.LogInformation(content);
            var model = JsonSerializer.Deserialize<PersonCheckResult>(content, _jsonOptions);
            personCheckResponse.PersonCheckResult = model;
        }
        else
        {
            if (string.IsNullOrEmpty(content))
            {
                _logger.LogError(content);
                result.EnsureSuccessStatusCode();
            }
            var model = JsonSerializer.Deserialize<PersonCheckError>(content!, _jsonOptions);
            personCheckResponse.ErrorResponse = model;
        }

        personCheckResponse.HttpStatusCode = (int)result.StatusCode;

        return personCheckResponse;
    }
}
