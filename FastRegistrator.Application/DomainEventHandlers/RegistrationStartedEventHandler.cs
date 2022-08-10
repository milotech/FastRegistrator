using FastRegistrator.ApplicationCore.Commands.CheckPerson;
using FastRegistrator.ApplicationCore.Domain.Events;
using FastRegistrator.ApplicationCore.Interfaces;
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
        private readonly ICommandExecutor _cmdExecutor;

        public RegistrationStartedCommittedEventHandler(
            ICommandExecutor cmdExecutor,
            ILogger<RegistrationStartedCommittedEventHandler> logger            
        )
        {
            _cmdExecutor = cmdExecutor;
            _logger = logger;            
        }

        public Task Handle(RegistrationStartedEvent @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Registration '{@event.Registration.Id}' started.");

            var command = new CheckPersonCommand(
                Name: @event.Registration.PersonData.Name,
                PassportNumber: @event.Registration.PersonData.Passport,
                INN: null,
                BirthDt: null
                );
            _cmdExecutor.Execute(command);

            _logger.LogInformation($"Registration '{@event.Registration.Id}' sent to KonturPrizma check queue.");

            return Task.CompletedTask;
        }
    }
}
