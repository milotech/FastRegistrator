using FastRegistrator.ApplicationCore.Domain.Events;
using FastRegistrator.ApplicationCore.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text;

namespace FastRegistrator.ApplicationCore.DomainEventHandlers
{
    public class RegistrationCompletedCommittedEventHandler : INotificationHandler<RegistrationCompletedEvent>
    {
        private readonly ILogger _logger;

        public RegistrationCompletedCommittedEventHandler(ILogger<RegistrationCompletedCommittedEventHandler> logger)
        {
            _logger = logger;            
        }

        public Task Handle(RegistrationCompletedEvent @event, CancellationToken cancellationToken)
        {
            string logMessage = $"Registration '{@event.Registration.Id}' completed with status '{@event.Registration.StatusHistory.Last().Status}'";
            if (@event.Registration.Error != null)
                logMessage += Environment.NewLine + $"Error from {@event.Registration.Error.Source}: {@event.Registration.Error.Message}";

            _logger.LogInformation(logMessage);

            return Task.CompletedTask;
        }
    }
}
