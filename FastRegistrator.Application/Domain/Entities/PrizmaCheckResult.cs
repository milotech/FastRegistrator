using FastRegistrator.Application.Domain.Enums;

namespace FastRegistrator.Application.Domain.Entities
{
    public class PrizmaCheckResult : BaseEntity<Guid>
    {
        public bool Result => RejectionReasonCode == RejectionReason.None;

        public RejectionReason RejectionReasonCode { get; private set; }
        public string PrizmaResponse { get; private set; }

        public PrizmaCheckResult(RejectionReason rejectionReasonCode, string prizmaResponse)
        {
            RejectionReasonCode = rejectionReasonCode;
            PrizmaResponse = prizmaResponse;
        }
    }
}
