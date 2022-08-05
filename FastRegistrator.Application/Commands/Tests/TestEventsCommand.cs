using FastRegistrator.ApplicationCore.Attributes;
using FastRegistrator.ApplicationCore.Domain.Entities;
using FastRegistrator.ApplicationCore.Domain.ValueObjects;
using FastRegistrator.ApplicationCore.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FastRegistrator.ApplicationCore.Commands.Tests
{
    [Command(CommandExecutionMode.InPlace)]
    public record TestEventsCommand(string PhoneNumber, string FirstName, string LastName) : IRequest;

    public class TestEventsCommandHandler: AsyncRequestHandler<TestEventsCommand>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger _logger;

        public TestEventsCommandHandler(IApplicationDbContext dbContext, ILogger<TestEventsCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        protected override async Task Handle(TestEventsCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("TestEventsCommand Started.");

            var name = new PersonName(request.FirstName, null, request.LastName);
            var passport = new Passport("0000", "000000", "test", new DateTime(1900, 01, 01), "test", "RUS");
            var personData = new PersonData(name, passport, "123456789", "{}");
            var registration = new Registration(request.PhoneNumber, personData);

            _logger.LogInformation("New Registration created: " + registration.Id);

            _dbContext.Registrations.Add(registration);

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("TestEventsCommand Completed.");
        }
    }
}
