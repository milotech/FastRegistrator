using FastRegistrator.Application.Domain.Enums;

namespace FastRegistrator.Application.DTOs.RegistrationStatusQuery
{
    public record class RegistrationStatusResponse
    {
        public Guid RegistrationId { get; init; }
        public bool Completed { get; init; }
        public RegistrationStatus Status { get; init; }
        public RejectionReason? PrizmaRejectionReason { get; init; }
        public RegistrationError? Error { get; init; }

        public RegistrationStatusResponse(Guid registrationId, bool completed, RegistrationStatus status, RejectionReason? prizmaRejectionReason, RegistrationError? error)
        {
            RegistrationId = registrationId;
            Completed = completed;
            Status = status;
            PrizmaRejectionReason = prizmaRejectionReason;
            Error = error;
        }
    }
}