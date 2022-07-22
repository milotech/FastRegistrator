using FastRegistrator.Infrastructure.DTOs.PrizmaServiceDTOs;

namespace FastRegistrator.Infrastructure.Interfaces
{
    public interface IPrizmaService
    {
        Task<PersonCheckCommonResponse> PersonCheck(PersonCheckRequest personCheckRequest, CancellationToken cancelToken);
    }
}
