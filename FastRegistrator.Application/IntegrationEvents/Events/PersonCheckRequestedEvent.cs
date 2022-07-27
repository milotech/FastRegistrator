using FastRegistrator.ApplicationCore.Interfaces;

namespace FastRegistrator.ApplicationCore.IntegrationEvents.Events
{
    public class PersonCheckRequestedEvent : IIntegrationEvent
    {
        public string PhoneNumber { get; init; } = null!;
    }
}
