using FastRegistrator.ApplicationCore.Domain.Entities;
using FastRegistrator.ApplicationCore.Domain.ValueObjects;
using FastRegistrator.ApplicationCore.Exceptions;
using FastRegistrator.ApplicationCore.Queries;
using Microsoft.Extensions.Logging;
using Moq;
using System.ComponentModel;
using static FastRegistrator.Tests.Constants;

namespace FastRegistrator.Tests.Queries
{
    public class GetRegistrationStatusQueryHandlerTest : TestWithDbContext
    {
        [Fact]
        [Description("Arrange GetStatus action trying to get status of registration that exists" +
                     "Act Handler for GetRegistrationStatusQuery is called" +
                     "Assert Handler doesn't throw exception")]
        public async Task Handle_GetStatusForExsistingRegistration_DoesntThrowException()
        {
            // Arrange
            var logger = new Mock<ILogger<GetRegistrationStatusQueryHandler>>();
            using var context = CreateDbContext();

            var personData = ConstructPersonData();
            var registration = new Registration(GUID, PHONE_NUMBER, personData);

            var entityEntry = context.Registrations.Add(registration);
            await context.SaveChangesAsync();

            var handler = new GetRegistrationStatusQueryHandler(context, logger.Object);
            var query = new GetRegistrationStatusQuery(entityEntry.Entity.Id);

            // Act
            var exception = await Record.ExceptionAsync(() => handler.Handle(query, CancellationToken.None));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        [Description("Arrange GetStatus action trying to get status of registration that doesn't exist" +
                     "Act Handler for GetRegistrationStatusQuery is called" +
                     "Assert Handler throws exception")]
        public async Task Handle_GetStatusForNonExsistentRegistration_ThrowsException()
        {
            // Arrange
            var logger = new Mock<ILogger<GetRegistrationStatusQueryHandler>>();
            using var context = CreateDbContext();

            var handler = new GetRegistrationStatusQueryHandler(context, logger.Object);
            var query = new GetRegistrationStatusQuery(GUID);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }

        private PersonData ConstructPersonData()
        {
            var personName = new PersonName(FIRST_NAME, MIDDLE_NAME, LAST_NAME);
            var personData = new PersonData(personName, PHONE_NUMBER, PASSPORT_NUMBER, BIRTHDAY, INN, FORM_DATA);

            return personData;
        }
    }
}