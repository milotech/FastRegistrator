namespace FastRegistrator.ApplicationCore.Interfaces
{
    public interface IEventBus
    {
        // TODO: specify number of consumer instances for parallel processing an events queue
        void Subscribe<TEvent, THandler>()
            where TEvent : IIntegrationEvent
            where THandler : IIntegrationEventHandler<TEvent>;

        void Unsubscribe<TEvent, THandler>()
            where TEvent : IIntegrationEvent
            where THandler : IIntegrationEventHandler<TEvent>;

        void Publish(IIntegrationEvent integrationEvent);
    }
}
