using FastRegistrator.ApplicationCore.IntegrationEvents.Events;
using FastRegistrator.ApplicationCore.IntegrationEvents.Handlers;
using FastRegistrator.ApplicationCore.Interfaces;
using FastRegistrator.Infrastructure.Interfaces;


public static class UseEventBusExtension
{
    public static void UseEventBus(this IApplicationBuilder app)
    {
        var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

        eventBus.Subscribe<TestIntegrationEvent, TestIntegrationEventHandler>();
    }
}

