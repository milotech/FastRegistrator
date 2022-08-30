using FastRegistrator.Application.Commands.SendDataToIC;
using FastRegistrator.Application.Domain.Events;
using FastRegistrator.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FastRegistrator.Application.DomainEventHandlers
{
    public class PrizmaCheckPassedCommittedEventHandler : INotificationHandler<CommittedEvent<PrizmaCheckPassedEvent>>
    {
        private readonly ILogger _logger;
        private readonly ICommandExecutor _cmdExecutor;

        public PrizmaCheckPassedCommittedEventHandler(
            ILogger<PrizmaCheckPassedCommittedEventHandler> logger,
            ICommandExecutor cmdExecutor
        )
        {
            _logger = logger;
            _cmdExecutor = cmdExecutor;
        }

        public Task Handle(CommittedEvent<PrizmaCheckPassedEvent> committedEvent, CancellationToken cancellationToken)
        {
            var @event = committedEvent.Event;

            _logger.LogInformation($"Prizma check passed for '{@event.Registration.Id}'");

            var command = new SendDataToICCommand(@event.Registration.Id);
            _cmdExecutor.Execute(command);

            return Task.CompletedTask;
        }
    }
}
