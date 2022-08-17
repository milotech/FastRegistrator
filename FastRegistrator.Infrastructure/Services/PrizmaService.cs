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
        var uriRequest = $"{PERSON_CHECK_PATH}?Fio={personCheckRequest.Fio}&PassportNumber={personCheckRequest.PassportNumber}&Inn={personCheckRequest.Inn}";
        if (personCheckRequest.DateOfBirth.HasValue)
        {
            uriRequest += $"&DateOfBirth={personCheckRequest.DateOfBirth.Value:yyyy’-‘MM’-‘dd’}";
        }
        var result = await _httpClient.GetAsync(uriRequest, cancelToken);
        var content = await result.Content.ReadAsStringAsync(cancelToken);

        var personCheckCommonResponse = new PersonCheckCommonResponse();

        if (result.IsSuccessStatusCode)
        {
            var model = JsonSerializer.Deserialize<PersonCheckResult>(content);
            personCheckCommonResponse.PersonCheckResult = model;
        }
        else
        {
            if(string.IsNullOrEmpty(content))
            {
                result.EnsureSuccessStatusCode();
            }
            var model = JsonSerializer.Deserialize<ErrorResponse>(content!);
            personCheckCommonResponse.ErrorResponse = model;
        }

        return personCheckCommonResponse;
    }
}
