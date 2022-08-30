using FastRegistrator.ApplicationCore.DTOs.PrizmaService;

namespace FastRegistrator.ApplicationCore.Interfaces
{
    public interface IPrizmaService
    {
        Task<PersonCheckResponse> PersonCheck(PersonCheckRequest personCheckRequest, CancellationToken cancelToken);
    }
}
