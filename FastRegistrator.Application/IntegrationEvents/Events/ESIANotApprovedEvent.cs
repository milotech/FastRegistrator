using FastRegistrator.ApplicationCore.Interfaces;

namespace FastRegistrator.ApplicationCore.IntegrationEvents.Events
{
    public class ESIANotApprovedEvent : IIntegrationEvent
    {
        public string PhoneNumber { get; init; } = null!;
        public string? Message { get; init; }
    }
}
