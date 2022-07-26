using FastRegistrator.ApplicationCore.DTOs.PrizmaServiceDTOs;

namespace FastRegistrator.ApplicationCore.Interfaces
{
    public interface IPrizmaService
    {
        Task<PersonCheckCommonResponse> PersonCheck(PersonCheckRequest personCheckRequest, CancellationToken cancelToken);
    }
}
