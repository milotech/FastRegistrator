using FastRegistrator.ApplicationCore.Interfaces;

namespace FastRegistrator.Infrastructure.Interfaces
{
    public interface IEventBus : IIntegrationEventsService
    {
        // TODO: specify number of consumer instances for parallel processing an events queue
        void Subscribe<T, TH>()
            where T : IIntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        void Unsubscribe<T, TH>()
            where T : IIntegrationEvent
            where TH : IIntegrationEventHandler<T>;
    }
}
