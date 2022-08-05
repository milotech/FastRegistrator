using FastRegistrator.ApplicationCore.Commands.SetStatusESIAApproved;
using FastRegistrator.ApplicationCore.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.ComponentModel;

namespace FastRegistrator.Tests.Commands
{
    public class StartRegistrationCommandHandlerTest : TestWithDbContext
    {
        public static readonly Guid GUID = new Guid("00000000-0000-0000-0000-000000000000");
        public const string PERSON_PHONE_NUMBER = "+79999999999";
        public const string FIRST_NAME = "Ivan";
        public const string LAST_NAME = "Ivanov";
        public const string MIDDLE_NAME = "Ivanovich";
        public const string SERIES = "1111";
        public const string NUMBER = "111111";
        public const string ISSUED_BY = "Department of the Federal Migration Service of Russia";
        public static readonly DateTime ISSUE_DATE = DateTime.MinValue;
        public const string ISSUE_ID = "111-111";
        public const string CITIZENSHIP = "Russia";
        public const string SNILS = "111-111-111 11";
        public const string FORM_DATA = "Example of FormData";

        [Fact]
        [Description("Arrange RegistrationStartedEvent is called" +
                     "Act Handler for StartRegistrationCommand is called" +
                     "Assert Add new registration to database with appropriate status")]
        public async Task Handle_RegistrationStartedEventiIsCalled_RegistrationIsAddedToDatabaseWithStatus()
        {
            // Arrange
            var logger = new Mock<ILogger<StartRegistrationCommandHandler>>();
            using var context = CreateDbContext();
            IRequestHandler<StartRegistrationCommand> handler = new StartRegistrationCommandHandler(context, logger.Object);
            var command = new StartRegistrationCommand
            {
                Guid = GUID,
                PhoneNumber = PERSON_PHONE_NUMBER,
                FirstName = FIRST_NAME,
                LastName = LAST_NAME,
                MiddleName = MIDDLE_NAME,
                Series = SERIES,
                Number = NUMBER,
                IssuedBy = ISSUED_BY,
                IssueDate = ISSUE_DATE,
                IssueId = ISSUE_ID,
                Citizenship = CITIZENSHIP,
                Snils = SNILS,
                FormData = FORM_DATA
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            var assertPerson = await context.Registrations
                .Include(p => p.StatusHistory.OrderByDescending(shi => shi.StatusDT).Take(1))
                .FirstOrDefaultAsync(p => p.PhoneNumber == PERSON_PHONE_NUMBER);

            Assert.NotNull(assertPerson);
            Assert.Contains(assertPerson!.StatusHistory, shi => shi.Status == RegistrationStatus.ClientFilledApplication);
        }

        [Fact]
        [Description("Arrange RegistrationStartedEvent is called" +
                     "Act Handler for StartRegistrationCommand is called" +
                     "Assert Add new registration to database with person data")]
        public async Task Handle_RegistrationStartedEventiIsCalled_RegistrationIsAddedToDatabaseWithPersonData()
        {
            // Arrange
            var logger = new Mock<ILogger<StartRegistrationCommandHandler>>();
            using var context = CreateDbContext();
            IRequestHandler<StartRegistrationCommand> handler = new StartRegistrationCommandHandler(context, logger.Object);
            var command = new StartRegistrationCommand
            {
                Guid = GUID,
                PhoneNumber = PERSON_PHONE_NUMBER,
                FirstName = FIRST_NAME,
                LastName = LAST_NAME,
                MiddleName = MIDDLE_NAME,
                Series = SERIES,
                Number = NUMBER,
                IssuedBy = ISSUED_BY,
                IssueDate = ISSUE_DATE,
                IssueId = ISSUE_ID,
                Citizenship = CITIZENSHIP,
                Snils = SNILS,
                FormData = FORM_DATA
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            var assertPerson = await context.Registrations
                .Include(p => p.StatusHistory.OrderByDescending(shi => shi.StatusDT).Take(1))
                .FirstOrDefaultAsync(p => p.PhoneNumber == PERSON_PHONE_NUMBER);

            Assert.NotNull(assertPerson);
            Assert.NotNull(assertPerson!.PersonData);
        }
    }
}
