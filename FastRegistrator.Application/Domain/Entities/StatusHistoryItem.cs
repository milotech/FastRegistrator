using FastRegistrator.Application.Domain.Enums;

namespace FastRegistrator.Application.Domain.Entities
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
