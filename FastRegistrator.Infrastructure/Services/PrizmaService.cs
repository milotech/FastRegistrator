using FastRegistrator.Infrastructure.DTOs.PrizmaServiceDTOs;
using FastRegistrator.Infrastructure.Interfaces;
using System.Text.Json;

namespace FastRegistrator.Infrastructure.Services;

public class PrizmaService : IPrizmaService
{
    private readonly HttpClient _httpClient;
    private const string PERSON_CHECK_PATH = "PersonCheck/Check";
    public PrizmaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PersonCheckCommonResponse> PersonCheck(PersonCheckRequest personCheckRequest, CancellationToken cancelToken)
    {
        var request = JsonSerializer.Serialize(personCheckRequest);
        var result = await _httpClient.PostAsync(PERSON_CHECK_PATH, new StringContent(request), cancelToken);
        var content = await result.Content.ReadAsStringAsync(cancelToken);

        var personCheckCommonResponse = new PersonCheckCommonResponse();

        if (result.IsSuccessStatusCode)
        {
            var model = JsonSerializer.Deserialize<PersonCheckResult>(content);
            personCheckCommonResponse.PersonCheckResult = model;
        }
        else
        {
            var model = JsonSerializer.Deserialize<ErrorResponse>(content);
            personCheckCommonResponse.ErrorResponse = model;
        }

        return personCheckCommonResponse;
    }
}
