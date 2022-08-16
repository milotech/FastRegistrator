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

        public Task Handle(CommittedEvent<PrizmaCheckPassedEvent> @event, CancellationToken cancellationToken)
        {          
            _logger.LogInformation($"Prizma check passed for '{@event.Event.Registration.Id}'");

            // _cmdExecutor.Execute( send data to IC command )

            return Task.CompletedTask;
        }
    }
}
