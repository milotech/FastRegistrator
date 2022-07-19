namespace FastRegistrator.ApplicationCore.Domain.Entities
{
    public class PrizmaCheckResult
    {
        public int Id { get; set; }
        public int StatusHistoryItemId { get; set; }
        public bool Result { get; set; }
        public RejectionReason RejectionReasonCode { get; set; }
        public string PrizmaResponse { get; set; }
    }
}
