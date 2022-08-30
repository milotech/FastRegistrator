using FastRegistrator.Application.Commands.CheckPerson;
using FastRegistrator.Application.Domain.Events;
using FastRegistrator.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FastRegistrator.Application.DomainEventHandlers
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

    public class RegistrationStartedCommittedEventHandler : INotificationHandler<CommittedEvent<RegistrationStartedEvent>>
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

        public Task Handle(CommittedEvent<RegistrationStartedEvent> committedEvent, CancellationToken cancellationToken)
        {
            var @event = committedEvent.Event;

            _logger.LogInformation($"Registration '{@event.Registration.Id}' started.");

            var command = new CheckPersonCommand(
                RegistrationId: @event.Registration.Id,
                Name: @event.Registration.PersonData.Name,
                PassportNumber: @event.Registration.PersonData.PassportNumber,
                INN: @event.Registration.PersonData.Inn,
                BirthDt: @event.Registration.PersonData.BirthDay
                );

            _cmdExecutor.Execute(command);

            _logger.LogInformation($"Registration '{@event.Registration.Id}' sent to KonturPrizma check queue.");

            return Task.CompletedTask;
        }
    }
}
