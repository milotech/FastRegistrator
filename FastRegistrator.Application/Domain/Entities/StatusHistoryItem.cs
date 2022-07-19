namespace FastRegistrator.ApplicationCore.Domain.Entities
{
    public class StatusHistoryItem
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public DateTime StatusDT { get; set; }
        public RegistrationStatus StatusCode { get; set; }
    }
}
