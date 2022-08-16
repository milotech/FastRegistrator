using FastRegistrator.ApplicationCore.Attributes;
using FastRegistrator.ApplicationCore.Domain.Entities;
using FastRegistrator.ApplicationCore.Domain.ValueObjects;
using FastRegistrator.ApplicationCore.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FastRegistrator.ApplicationCore.Commands.StartRegistration
{
    [Command(CommandExecutionMode.InPlace)]
    public record class StartRegistrationCommand : IRequest
    {
        public Guid RegistrationId { get; init; }
        public string PhoneNumber { get; init; } = null!;
        public string FirstName { get; init; } = null!;
        public string? MiddleName { get; init; }
        public string LastName { get; init; } = null!;
        public string Series { get; init; } = null!;
        public string Number { get; init; } = null!;
        public string IssuedBy { get; init; } = null!;
        public DateTime IssueDate { get; init; }
        public string IssueId { get; init; } = null!;
        public string Citizenship { get; init; } = null!;
        public string Snils { get; init; } = null!;
        public string FormData { get; init; } = null!;

        public override string ToString()
        {
            return nameof(StartRegistrationCommand) + $" {{ RegistrationId = {RegistrationId} }}";
        }
    }

    public class StartRegistrationCommandHandler : AsyncRequestHandler<StartRegistrationCommand>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<StartRegistrationCommandHandler> _logger;

        public StartRegistrationCommandHandler(IApplicationDbContext dbContext, ILogger<StartRegistrationCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        protected override async Task Handle(StartRegistrationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Registration for person with phone number '{request.PhoneNumber}' is begun.");

            var registration = new Registration(request.RegistrationId, request.PhoneNumber, ConstructPersonData(request));
            _dbContext.Registrations.Add(registration);

            await _dbContext.SaveChangesAsync();
        }

        private PersonData ConstructPersonData(StartRegistrationCommand request) 
        {
            var personName = new PersonName(request.FirstName, request.MiddleName, request.LastName);
            var passport = new Passport(request.Series, request.Number, request.IssuedBy, request.IssueDate, request.IssueId, request.Citizenship);
            var personData = new PersonData(personName, passport, request.Snils, request.FormData);

            return personData;
        }
    }
}
