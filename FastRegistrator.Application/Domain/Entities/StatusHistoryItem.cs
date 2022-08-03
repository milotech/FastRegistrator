using FastRegistrator.ApplicationCore.Domain.Enums;

namespace FastRegistrator.ApplicationCore.Domain.Entities
{
    public class StatusHistoryItem : BaseEntity<int>
    {
        public RegistrationStatus Status { get; private set; }
        public DateTime StatusDT { get; private set; } 
        public PrizmaCheckResult? PrizmaCheckResult { get; private set; }
        public PrizmaCheckError? PrizmaCheckError { get; private set; }

        public StatusHistoryItem(RegistrationStatus status)
        {
            Status = status;
            StatusDT = DateTime.UtcNow;
        }

        public static StatusHistoryItem FromPrizmaCheckResult(PrizmaCheckResult prizmaCheckResult)
        {
            var status = prizmaCheckResult.Result
                ? RegistrationStatus.PrizmaCheckSuccessful
                : RegistrationStatus.PrizmaCheckRejected;

            return new StatusHistoryItem(status)
            {
                PrizmaCheckResult = prizmaCheckResult
            };
        }

        public static StatusHistoryItem FromPrizmaCheckError(PrizmaCheckError prizmaCheckError)
        {
            return new StatusHistoryItem(RegistrationStatus.PrizmaCheckFailed)
            {
                PrizmaCheckError = prizmaCheckError
            };
        }
    }
}
