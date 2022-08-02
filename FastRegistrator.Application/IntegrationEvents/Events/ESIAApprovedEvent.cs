using FastRegistrator.ApplicationCore.Interfaces;

namespace FastRegistrator.ApplicationCore.IntegrationEvents.Events
{
    public class ESIAApprovedEvent : IIntegrationEvent
    {
        public string PhoneNumber { get; init; } = null!;
        public string FirstName { get; init; } = null!;
        public string? MiddleName { get; init; }
        public string LastName { get; init; } = null!;
        public string Series { get; init; } = null!;
        public string Number { get; init; } = null!;
        public string IssuedBy { get; init; } = null!;
        public DateTime IssueDate { get; init; }
        public string IssueId { get; init; } = null!;
        public string Citizenship { get; init; } = null!;
        public string Snils { get; init; } = null!;
    }
}
