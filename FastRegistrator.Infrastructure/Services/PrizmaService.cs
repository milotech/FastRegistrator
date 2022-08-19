using FastRegistrator.ApplicationCore.DTOs.PrizmaServiceDTOs;
using FastRegistrator.ApplicationCore.Interfaces;
using Newtonsoft.Json;

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

        if (result.IsSuccessStatusCode)
        {
            var model = JsonConvert.DeserializeObject<PersonCheckResult>(content);
            personCheckCommonResponse.PersonCheckResult = model;
        }
        else
        {
            if (string.IsNullOrEmpty(content))
            {
                result.EnsureSuccessStatusCode();
            }
            var model = JsonConvert.DeserializeObject<ErrorResponse>(content!);
            personCheckCommonResponse.ErrorResponse = model;
        }

        personCheckCommonResponse.HttpStatusCode = (int)result.StatusCode;

        return personCheckCommonResponse;
    }
}
