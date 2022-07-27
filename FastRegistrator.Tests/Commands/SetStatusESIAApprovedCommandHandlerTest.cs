using FastRegistrator.ApplicationCore.Commands.SetStatusESIAApproved;
using FastRegistrator.ApplicationCore.Domain.Entities;
using FastRegistrator.ApplicationCore.Domain.Enums;
using Microsoft.Extensions.Logging;
using Moq;
using System.ComponentModel;

namespace FastRegistrator.Tests.Commands
{
    public class SetStatusESIAApprovedCommandHandlerTest : TestWithDbContext
    {
        private const string PERSON_PHONE_NUMBER = "+79999999999";
        private const string FIRST_NAME = "Ivan";
        private const string LAST_NAME = "Ivanov";
        private const string MIDDLE_NAME = "Ivanovich";
        private const string SERIES = "1111";
        private const string NUMBER = "111111";
        private const string ISSUED_BY = "Department of the Federal Migration Service of Russia";
        private static readonly DateTime ISSUE_DATE = DateTime.MinValue;
        private const string ISSUE_ID = "111-111";
        private const string CITIZENSHIP = "Russia";
        private const string SNILS = "111-111-111 11";

        [Fact]
        [Description("Arrange Person doesn't exist in database" +
                     "Act Person is approved by ESIA check" +
                     "Assert Add new person to database")]
        public async Task Handle_PersonDoesntExistInDatabase_PersonWasAddedToDatabase()
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
            Assert.Contains(context.Persons.Local, p => p.PhoneNumber == PERSON_PHONE_NUMBER &&
                p.StatusHistory?.OrderByDescending(shi => shi.StatusDT).FirstOrDefault()?.Status == PersonStatus.ESIAApproved &&
                p.PersonData != null);
        }

        [Fact]
        [Description("Arrange Person exists in database, but passed check from Prizma more then 6 month ago" +
                     "Act Person is approved by ESIA check" +
                     "Assert Update exist person in database")]
        public async Task Handle_PersonExistsInDatabase_PersonWasUpdatedInDatabase()
        {
            // Arrange
            var logger = new Mock<ILogger<SetStatusESIAApprovedCommandHandler>>();
            using var context = CreateDbContext();

            var person = new Person(PERSON_PHONE_NUMBER);
            context.Persons.Add(person);
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
            Assert.Contains(context.Persons.Local, p => p.PhoneNumber == PERSON_PHONE_NUMBER &&
                p.StatusHistory?.OrderByDescending(shi => shi.StatusDT).FirstOrDefault()?.Status == PersonStatus.ESIAApproved &&
                p.PersonData != null);
        }
    }
}
