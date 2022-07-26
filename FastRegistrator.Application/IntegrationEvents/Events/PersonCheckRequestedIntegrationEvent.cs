using FastRegistrator.ApplicationCore.Interfaces;

namespace FastRegistrator.ApplicationCore.IntegrationEvents.Events
{
    public class PersonCheckRequestedIntegrationEvent : IIntegrationEvent
    {
        public string PhoneNumber { get; init; } = null!;
    }
}
