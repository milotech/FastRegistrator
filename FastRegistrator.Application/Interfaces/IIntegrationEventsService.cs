namespace FastRegistrator.ApplicationCore.Interfaces
{
    public interface IIntegrationEventsService
    {
        void Publish(IIntegrationEvent integrationEvent);
    }
}
