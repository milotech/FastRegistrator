using FastRegistrator.Application.DTOs.PrizmaService;

namespace FastRegistrator.Application.Interfaces
{
    public interface IPrizmaService
    {
        Task<PersonCheckResponse> PersonCheck(PersonCheckRequest personCheckRequest, CancellationToken cancelToken);
    }
}
