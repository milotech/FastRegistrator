using FastRegistrator.ApplicationCore.Commands.SendDataToIC;
using FastRegistrator.ApplicationCore.Domain.Events;
using FastRegistrator.ApplicationCore.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FastRegistrator.ApplicationCore.DomainEventHandlers
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

            _logger.LogInformation($"Person data for registration with Guid '{@event.Registration.Id}' sent to IC.");

            return Task.CompletedTask;
        }
    }
}
