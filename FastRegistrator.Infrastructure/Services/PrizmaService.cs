using FastRegistrator.ApplicationCore.DTOs.PrizmaServiceDTOs;
using FastRegistrator.ApplicationCore.Interfaces;
using System.Text;
using System.Text.Json;

namespace FastRegistrator.Infrastructure.Services;

public class PrizmaService : IPrizmaService
{
    private readonly HttpClient _httpClient;
    private const string PERSON_CHECK_PATH = "PersonCheck";
    private const string MEDIA_TYPE = "application/json";

    public PrizmaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PersonCheckCommonResponse> PersonCheck(PersonCheckRequest personCheckRequest, CancellationToken cancelToken)
    {
        var request = JsonSerializer.Serialize(personCheckRequest);
        var stringContent = new StringContent(request, Encoding.UTF8, MEDIA_TYPE);
        var result = await _httpClient.PostAsync(PERSON_CHECK_PATH, stringContent, cancelToken);
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
