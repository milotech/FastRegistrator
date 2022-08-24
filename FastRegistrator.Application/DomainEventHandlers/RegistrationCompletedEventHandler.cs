using FastRegistrator.ApplicationCore.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FastRegistrator.ApplicationCore.DomainEventHandlers
{
    public class RegistrationCompletedCommittedEventHandler : INotificationHandler<CommittedEvent<RegistrationCompletedEvent>>
    {
        private readonly ILogger _logger;

        public RegistrationCompletedCommittedEventHandler(ILogger<RegistrationCompletedCommittedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(CommittedEvent<RegistrationCompletedEvent> committedEvent, CancellationToken cancellationToken)
        {
            var @event = committedEvent.Event;

            string logMessage = $"Registration '{@event.Registration.Id}' completed with status '{@event.Registration.StatusHistory.Last().Status}'";
            if (@event.Registration.Error != null)
            {
                logMessage += Environment.NewLine + $"Error from {@event.Registration.Error.Source}: {@event.Registration.Error.Message}";
                if (@event.Registration.Error.Details != null)
                    logMessage += Environment.NewLine + $"ErrorDetails: " + @event.Registration.Error.Details;
            }

            _logger.LogInformation(logMessage);

            return Task.CompletedTask;
        }
    }
}
