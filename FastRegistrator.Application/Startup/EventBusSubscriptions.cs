using FastRegistrator.Application.Commands.StartRegistration;
using FastRegistrator.Application.IntegrationEvents.Events;
using FastRegistrator.Application.IntegrationEvents.Handlers;
using FastRegistrator.Application.Interfaces;
using MediatR;

namespace FastRegistrator.Application.Startup
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