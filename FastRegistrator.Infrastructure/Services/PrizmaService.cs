using FastRegistrator.ApplicationCore.DTOs.PrizmaServiceDTOs;
using FastRegistrator.ApplicationCore.Interfaces;
using System.Text.Json;

namespace FastRegistrator.Infrastructure.Services;

public class PrizmaService : IPrizmaService
{
    private readonly HttpClient _httpClient;
    private const string PERSON_CHECK_PATH = "PersonCheck";

    public PrizmaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PersonCheckCommonResponse> PersonCheck(PersonCheckRequest personCheckRequest, CancellationToken cancelToken)
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

        var personCheckCommonResponse = new PersonCheckCommonResponse();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        if (result.IsSuccessStatusCode)
        {
            var model = JsonSerializer.Deserialize<PersonCheckResult>(content, options);
            personCheckCommonResponse.PersonCheckResult = model;
        }
        else
        {
            if (string.IsNullOrEmpty(content))
            {
                result.EnsureSuccessStatusCode();
            }
            var model = JsonSerializer.Deserialize<ErrorResponse>(content!, options);
            personCheckCommonResponse.ErrorResponse = model;
        }

        personCheckCommonResponse.HttpStatusCode = (int)result.StatusCode;

        return personCheckCommonResponse;
    }
}
