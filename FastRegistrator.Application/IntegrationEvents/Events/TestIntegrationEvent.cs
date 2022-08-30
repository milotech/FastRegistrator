using FastRegistrator.Application.Interfaces;

namespace FastRegistrator.Application.IntegrationEvents.Events
{
    public class TestIntegrationEvent : IIntegrationEvent
    {
        public int Value { get; set; }
        public string Description { get; set; } = null!;
    }
}
