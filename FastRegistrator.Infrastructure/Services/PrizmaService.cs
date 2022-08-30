using FastRegistrator.ApplicationCore.DTOs.PrizmaService;
using FastRegistrator.ApplicationCore.Interfaces;
using System.Text.Json;

namespace FastRegistrator.Infrastructure.Services;

public class PrizmaService : IPrizmaService
{
    private static JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient;
    private const string PERSON_CHECK_PATH = "PersonCheck";

    public PrizmaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
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
        var result = await _httpClient.GetAsync(requestUri, cancelToken);
        var content = await result.Content.ReadAsStringAsync(cancelToken);

        var personCheckCommonResponse = new PersonCheckResponse();        

        if (result.IsSuccessStatusCode)
        {
            var model = JsonSerializer.Deserialize<PersonCheckResult>(content, _jsonOptions);
            personCheckCommonResponse.PersonCheckResult = model;
        }
        else
        {
            if (string.IsNullOrEmpty(content))
            {
                result.EnsureSuccessStatusCode();
            }
            var model = JsonSerializer.Deserialize<PersonCheckError>(content!, _jsonOptions);
            personCheckCommonResponse.ErrorResponse = model;
        }

        personCheckCommonResponse.HttpStatusCode = (int)result.StatusCode;

        return personCheckCommonResponse;
    }
}
