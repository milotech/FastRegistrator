using FastRegistrator.ApplicationCore.Domain.Enums;

namespace FastRegistrator.ApplicationCore.Domain.Entities
{
    public class StatusHistoryItem : BaseEntity
    {
        public PersonStatus Status { get; private set; }
        public DateTime StatusDT { get; private set; }        
        public PrizmaCheckResult? PrizmaCheck { get; private set; }

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
                PrizmaCheck = checkResult
            };
        }
    }
}
