using FastRegistrator.ApplicationCore.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FastRegistrator.ApplicationCore.DomainEventHandlers
{
    public class RegistrationStartedEventHandler : INotificationHandler<RegistrationStartedEvent>
    {
        private readonly ILogger _logger;

        public RegistrationStartedEventHandler(ILogger<RegistrationStartedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(RegistrationStartedEvent @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Set status PrizmaCheckInProgress");

            var registration = @event.Registration;
            registration.SetPrizmaCheckInProgress();

            return Task.CompletedTask;
        }
    }

    public class RegistrationStartedCommittedEventHandler : INotificationHandler<RegistrationStartedEvent>
    {
        private readonly ILogger _logger;

        public RegistrationStartedCommittedEventHandler(ILogger<RegistrationStartedCommittedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(RegistrationStartedEvent @event, CancellationToken cancellationToken)
        {
            // send registration data to PrizmaCheckQueue

            _logger.LogInformation($"Registration '{@event.Registration.Id}' sent to KonturPrizma check queue.");

            return Task.CompletedTask;
        }
    }
}
