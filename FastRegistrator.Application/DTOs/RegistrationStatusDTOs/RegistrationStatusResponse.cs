using FastRegistrator.ApplicationCore.Domain.Enums;

namespace FastRegistrator.ApplicationCore.DTOs.GetStatusDTOs
{
    public class RegistrationStatusResponse
    {
        public Guid RegistrationId { get; private set; }
        public bool Completed { get; private set; }
        public RegistrationStatus Status { get; private set; }
        public RejectionReason? PrizmaRejectionReason { get; private set; }
        public ICResult? ICResult { get; private set; }
        public Error? Error { get; private set; }

        public RegistrationStatusResponse(Guid registrationId, bool completed, RegistrationStatus status, RejectionReason? prizmaRejectionReason, ICResult? icResult, Error? error)
        {
            RegistrationId = registrationId;
            Completed = completed;
            Status = status;
            PrizmaRejectionReason = prizmaRejectionReason;
            ICResult = icResult;
            Error = error;
        }
    }
}