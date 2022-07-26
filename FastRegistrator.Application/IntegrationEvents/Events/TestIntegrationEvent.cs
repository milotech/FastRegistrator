using FastRegistrator.ApplicationCore.Interfaces;

namespace FastRegistrator.ApplicationCore.IntegrationEvents.Events
{
    public class TestIntegrationEvent : IIntegrationEvent
    {
        public int Value { get; set; }
        public string Description { get; set; } = null!;
    }
}
