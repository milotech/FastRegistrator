using FastRegistrator.ApplicationCore.Domain.Enums;

namespace FastRegistrator.ApplicationCore.Domain.Entities
{
    public class StatusHistoryItem : BaseEntity<int>
    {
        public RegistrationStatus Status { get; private set; }
        public DateTime StatusDT { get; private set; } 

        public StatusHistoryItem(RegistrationStatus status)
        {
            Status = status;
            StatusDT = DateTime.UtcNow;
        }
    }
}
