using FastRegistrator.ApplicationCore.Commands.SetStatusESIAApproved;
using FastRegistrator.ApplicationCore.IntegrationEvents.Events;
using FastRegistrator.ApplicationCore.IntegrationEvents.Handlers;
using FastRegistrator.ApplicationCore.Interfaces;
using MediatR;

namespace FastRegistrator.ApplicationCore.Startup
{
    public static class EventBusSubscriptions
    {
        public static void StartApplicationSubscriptions(this IEventBus eventBus)
        {
            eventBus.SubscribeWithCommand<ESIAApprovedEvent, StartRegistrationCommand>();

            eventBus.Subscribe<TestIntegrationEvent, TestIntegrationEventHandler>();
        }

        private static void SubscribeWithCommand<TEvent, TCommand>(this IEventBus eventBus)
            where TEvent : IIntegrationEvent
            where TCommand : IBaseRequest
        {
            eventBus.Subscribe<TEvent, CommandBoundIntegrationEventHandler<TEvent, TCommand>>();
        }
    }
}