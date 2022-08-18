using FastRegistrator.ApplicationCore.Domain.Enums;

namespace FastRegistrator.ApplicationCore.DTOs.RegistrationStatusDTOs
{
    public record class RegistrationStatusResponse
    {
        public Guid RegistrationId { get; init; }
        public bool Completed { get; init; }
        public RegistrationStatus Status { get; init; }
        public RejectionReason? PrizmaRejectionReason { get; init; }
        public RegistrationAccountData? AccountData { get; init; }
        public RegistrationError? Error { get; init; }

        public RegistrationStatusResponse(Guid registrationId, bool completed, RegistrationStatus status, RejectionReason? prizmaRejectionReason, RegistrationAccountData? accountData, RegistrationError? error)
        {
            RegistrationId = registrationId;
            Completed = completed;
            Status = status;
            PrizmaRejectionReason = prizmaRejectionReason;
            AccountData = accountData;
            Error = error;
        }
    }
}