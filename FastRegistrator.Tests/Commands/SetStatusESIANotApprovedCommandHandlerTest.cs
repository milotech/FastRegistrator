using FastRegistrator.ApplicationCore.Commands.SetStatusESIANotApproved;
using FastRegistrator.ApplicationCore.Domain.Entities;
using FastRegistrator.ApplicationCore.Domain.Enums;
using Microsoft.Extensions.Logging;
using Moq;
using System.ComponentModel;

namespace FastRegistrator.Tests.Commands
{
    public class SetStatusESIANotApprovedCommandHandlerTest : TestWithDbContext
    {
        const string PERSON_PHONE_NUMBER = "+79999999999";

        [Fact]
        [Description("Arrange Person doesn't exist in database" +
                     "Act Person isn't approved by ESIA check" +
                     "Assert Add new person to database")]
        public async Task Handle_PersonDoesntExistInDatabase_PersonWasAddedToDatabase()
        {
            // Arrange
            var logger = new Mock<ILogger<SetStatusESIANotApprovedCommandHandler>>();
            using var context = CreateDbContext();
            var handler = new SetStatusESIANotApprovedCommandHandler(context, logger.Object);
            var command = new SetStatusESIANotApprovedCommand
            {
                PhoneNumber = PERSON_PHONE_NUMBER,
                RejectReason = string.Empty
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Contains(context.Persons.Local, p => p.PhoneNumber == PERSON_PHONE_NUMBER &&
                p.StatusHistory?.OrderByDescending(shi => shi.StatusDT).FirstOrDefault()?.Status == PersonStatus.ESIANotApproved);
        }

        [Fact]
        [Description("Arrange Person exists in database, but passed check from Prizma more then 6 month ago" +
                     "Act Person isn't approved by ESIA check" +
                     "Assert Update exist person in database")]
        public async Task Handle_PersonExistsInDatabase_PersonWasUpdatedInDatabase()
        {
            // Arrange
            var logger = new Mock<ILogger<SetStatusESIANotApprovedCommandHandler>>();
            using var context = CreateDbContext();

            var person = new Person(PERSON_PHONE_NUMBER);
            context.Persons.Add(person);
            await context.SaveChangesAsync();

            var handler = new SetStatusESIANotApprovedCommandHandler(context, logger.Object);
            var command = new SetStatusESIANotApprovedCommand
            {
                PhoneNumber = PERSON_PHONE_NUMBER,
                RejectReason = string.Empty
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Contains(context.Persons.Local, p => p.PhoneNumber == PERSON_PHONE_NUMBER &&
                p.StatusHistory?.OrderByDescending(shi => shi.StatusDT).FirstOrDefault()?.Status == PersonStatus.ESIANotApproved);
        }
    }
}
