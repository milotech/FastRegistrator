using FastRegistrator.ApplicationCore.Commands.StartRegistration;
using FastRegistrator.ApplicationCore.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.ComponentModel;
using static FastRegistrator.Tests.Constants;

namespace FastRegistrator.Tests.Commands
{
    public class StartRegistrationCommandHandlerTest : TestWithDbContext
    {
        public StartRegistrationCommandHandlerTest() : base(true)
        { }

        [Fact]
        [Description("Arrange RegistrationStartedEvent is called" +
                     "Act Handler for StartRegistrationCommand is called" +
                     "Assert Add new registration to database with appropriate status")]
        public async Task Handle_RegistrationStartedEventIsCalled_RegistrationIsAddedToDatabaseWithStatus()
        {
            // Arrange
            var logger = new Mock<ILogger<StartRegistrationCommandHandler>>();
            using var context = CreateDbContext();
            IRequestHandler<StartRegistrationCommand> handler = new StartRegistrationCommandHandler(context, logger.Object);
            var command = new StartRegistrationCommand
            {
                RegistrationId = GUID,
                PhoneNumber = PHONE_NUMBER,
                FirstName = FIRST_NAME,
                LastName = LAST_NAME,
                MiddleName = MIDDLE_NAME,
                PassportNumber = PASSPORT_NUMBER,
                BirthDay = BIRTHDAY,
                Inn = INN,
                FormData = FORM_DATA
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            var assertPerson = await context.Registrations
                .Include(p => p.StatusHistory.OrderByDescending(shi => shi.StatusDT).Take(1))
                .FirstOrDefaultAsync(p => p.PhoneNumber == PHONE_NUMBER);

            Assert.NotNull(assertPerson);
            Assert.Contains(assertPerson!.StatusHistory, shi => shi.Status == RegistrationStatus.PersonDataReceived);
        }

        [Fact]
        [Description("Arrange RegistrationStartedEvent is called" +
                     "Act Handler for StartRegistrationCommand is called" +
                     "Assert Add new registration to database with person data")]
        public async Task Handle_RegistrationStartedEventIsCalled_RegistrationIsAddedToDatabaseWithPersonData()
        {
            // Arrange
            var logger = new Mock<ILogger<StartRegistrationCommandHandler>>();
            using var context = CreateDbContext();
            IRequestHandler<StartRegistrationCommand> handler = new StartRegistrationCommandHandler(context, logger.Object);
            var command = new StartRegistrationCommand
            {
                RegistrationId = GUID,
                PhoneNumber = PHONE_NUMBER,
                FirstName = FIRST_NAME,
                LastName = LAST_NAME,
                MiddleName = MIDDLE_NAME,
                PassportNumber = PASSPORT_NUMBER,
                BirthDay = BIRTHDAY,
                Inn = INN,
                FormData = FORM_DATA
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            var assertPerson = await context.Registrations
                .Include(p => p.StatusHistory.OrderByDescending(shi => shi.StatusDT).Take(1))
                .FirstOrDefaultAsync(p => p.PhoneNumber == PHONE_NUMBER);

            Assert.NotNull(assertPerson);
            Assert.NotNull(assertPerson!.PersonData);
        }
    }
}
