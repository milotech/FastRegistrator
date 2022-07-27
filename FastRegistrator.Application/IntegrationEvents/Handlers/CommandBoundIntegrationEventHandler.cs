using AutoMapper;
using FastRegistrator.ApplicationCore.Interfaces;
using MediatR;

namespace FastRegistrator.ApplicationCore.IntegrationEvents.Handlers
{
    public class CommandBoundIntegrationEventHandler<TEvent, TCommand> : IIntegrationEventHandler<TEvent>
        where TEvent : IIntegrationEvent
        where TCommand: IBaseRequest
    {
        private IMediator _mediator;
        private IMapper _mapper;

        public CommandBoundIntegrationEventHandler(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public Task Handle(TEvent integrationEvent, CancellationToken cancel)
        {
            var command = _mapper.Map<TEvent, TCommand>(integrationEvent);
            return _mediator.Send(command, cancel);
        }
    }
}
