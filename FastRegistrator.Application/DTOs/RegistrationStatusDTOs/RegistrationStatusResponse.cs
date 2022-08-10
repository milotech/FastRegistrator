using FastRegistrator.ApplicationCore.Domain.Enums;

namespace FastRegistrator.ApplicationCore.DTOs.GetStatusDTOs
{
    public class RegistrationStatusResponse
    {
        public Guid RegistrationId { get; private set; }
        public bool Completed { get; private set; }
        public RegistrationStatus Status { get; private set; }
        public RejectionReason? PrizmaRejectionReason { get; private set; }
        public RegistrationAccountData? RegistrationAccountData { get; private set; }
        public RegistrationError? RegistrationError { get; private set; }

        public RegistrationStatusResponse(Guid registrationId, bool completed, RegistrationStatus status, RejectionReason? prizmaRejectionReason, RegistrationAccountData? registrationAccountData, RegistrationError? registrationError)
        {
            RegistrationId = registrationId;
            Completed = completed;
            Status = status;
            PrizmaRejectionReason = prizmaRejectionReason;
            RegistrationAccountData = registrationAccountData;
            RegistrationError = registrationError;
        }
    }
}