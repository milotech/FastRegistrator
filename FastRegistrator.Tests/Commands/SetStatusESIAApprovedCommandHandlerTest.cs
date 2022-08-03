using FastRegistrator.ApplicationCore.Commands.SetStatusESIAApproved;
using FastRegistrator.ApplicationCore.Domain.Entities;
using FastRegistrator.ApplicationCore.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.ComponentModel;

namespace FastRegistrator.Tests.Commands
{
    public class SetStatusESIAApprovedCommandHandlerTest : TestWithDbContext
    {
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
        public const string FORM_DATA = "";

        [Fact]
        [Description("Arrange Person doesn't exist in database" +
                     "Act Person is approved by ESIA check" +
                     "Assert Add new person to database with appropriate status")]
        public async Task Handle_PersonDoesntExistInDatabase_PersonWasAddedToDatabaseWithStatus()
        {
            // Arrange
            var logger = new Mock<ILogger<SetStatusESIAApprovedCommandHandler>>();
            using var context = CreateDbContext();
            var handler = new SetStatusESIAApprovedCommandHandler(context, logger.Object);
            var command = new SetStatusESIAApprovedCommand
            {
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
                Snils = SNILS
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            var assertPerson = await context.Registrations
                .Include(p => p.StatusHistory.OrderByDescending(shi => shi.StatusDT).Take(1))
                .FirstOrDefaultAsync(p => p.PhoneNumber == PERSON_PHONE_NUMBER);

            Assert.NotNull(assertPerson);
        }

        [Fact]
        [Description("Arrange Person doesn't exist in database" +
                     "Act Person is approved by ESIA check" +
                     "Assert Add new person to database with person data")]
        public async Task Handle_PersonDoesntExistInDatabase_PersonWasAddedToDatabaseWithPersonData()
        {
            // Arrange
            var logger = new Mock<ILogger<SetStatusESIAApprovedCommandHandler>>();
            using var context = CreateDbContext();
            var handler = new SetStatusESIAApprovedCommandHandler(context, logger.Object);
            var command = new SetStatusESIAApprovedCommand
            {
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
                Snils = SNILS
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

        [Fact]
        [Description("Arrange Person exists in database, but passed check from Prizma more then 6 month ago" +
                     "Act Person is approved by ESIA check" +
                     "Assert Update exist person in database with appropriate status")]
        public async Task Handle_PersonExistsInDatabase_PersonWasUpdatedInDatabaseWithStatus()
        {
            // Arrange
            var logger = new Mock<ILogger<SetStatusESIAApprovedCommandHandler>>();
            using var context = CreateDbContext();

            var person = new Registration(PERSON_PHONE_NUMBER, ConstructPersonData());
            context.Registrations.Add(person);
            await context.SaveChangesAsync();

            var handler = new SetStatusESIAApprovedCommandHandler(context, logger.Object);
            var command = new SetStatusESIAApprovedCommand
            {
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
                Snils = SNILS
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            var assertPerson = await context.Registrations
                .Include(p => p.StatusHistory.OrderByDescending(shi => shi.StatusDT).Take(1))
                .FirstOrDefaultAsync(p => p.PhoneNumber == PERSON_PHONE_NUMBER);

            Assert.NotNull(assertPerson);
        }

        [Fact]
        [Description("Arrange Person exists in database, but passed check from Prizma more then 6 month ago" +
                     "Act Person is approved by ESIA check" +
                     "Assert Update exist person in database with person data")]
        public async Task Handle_PersonExistsInDatabase_PersonWasUpdatedInDatabaseWithPersonData()
        {
            // Arrange
            var logger = new Mock<ILogger<SetStatusESIAApprovedCommandHandler>>();
            using var context = CreateDbContext();

            var person = new Registration(PERSON_PHONE_NUMBER, ConstructPersonData());
            context.Registrations.Add(person);
            await context.SaveChangesAsync();

            var handler = new SetStatusESIAApprovedCommandHandler(context, logger.Object);
            var command = new SetStatusESIAApprovedCommand
            {
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
                Snils = SNILS
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

        private PersonData ConstructPersonData()
        {
            var personName = new PersonName(FIRST_NAME, MIDDLE_NAME, LAST_NAME);
            var passport = new Passport(SERIES, NUMBER, ISSUED_BY, ISSUE_DATE, ISSUE_ID, CITIZENSHIP);
            var personData = new PersonData(personName, passport, SNILS, FORM_DATA);

            return personData;
        }
    }
}
