using FastRegistrator.ApplicationCore.Domain.Events;
using FastRegistrator.ApplicationCore.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

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
            _logger.LogInformation($"Registration '{@event.Registration.Id}' completed with status '{@event.Registration.StatusHistory.Last().Status}'");

            return Task.CompletedTask;
        }
    }
}
