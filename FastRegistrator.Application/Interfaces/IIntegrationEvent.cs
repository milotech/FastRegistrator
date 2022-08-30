namespace FastRegistrator.Application.Interfaces
{
    public interface IIntegrationEvent
    {
        // maybe Id, CreatedDate
    }

    public interface IIntegrationEventHandler<in TIntegrationEvent>
        where TIntegrationEvent : IIntegrationEvent
    {
        Task Handle(TIntegrationEvent @event, CancellationToken cancel);
    }
}
