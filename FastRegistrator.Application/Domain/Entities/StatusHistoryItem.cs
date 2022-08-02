using FastRegistrator.ApplicationCore.Domain.Enums;

namespace FastRegistrator.ApplicationCore.Domain.Entities
{
    public class StatusHistoryItem : BaseEntity<Guid>
    {
        public PersonStatus Status { get; private set; }
        public DateTime StatusDT { get; private set; } 
        public PrizmaCheckResponse? PrizmaCheckResponse { get; private set; }

        public StatusHistoryItem(PersonStatus status)
        {
            Status = status;
            StatusDT = DateTime.UtcNow;
        }

        public static StatusHistoryItem FromPrizmaCheckResult(PrizmaCheckResult checkResult)
        {
            var status = checkResult.Result
                ? PersonStatus.PrizmaCheckSuccessful
                : PersonStatus.PrizmaCheckRejected;

            return new StatusHistoryItem(status)
            {
                PrizmaCheckResponse = new PrizmaCheckResponse(checkResult, null)
            };
        }
    }
}
