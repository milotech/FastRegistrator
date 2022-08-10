using FastRegistrator.ApplicationCore.Domain.Entities;
using FastRegistrator.ApplicationCore.Domain.ValueObjects;
using FastRegistrator.ApplicationCore.Exceptions;
using FastRegistrator.ApplicationCore.Queries.GetStatus;
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
                     "Assert Handler returns RegistrationStatusResponse")]
        public async Task Handle_GetStatusForExsistingRegistration_ReturnRegistrationStatusResponse()
        {
            // Arrange
            var logger = new Mock<ILogger<GetRegistrationStatusQueryHandler>>();
            using var context = CreateDbContext();

            var personData = ConstructPersonData();
            var registration = new Registration(GUID, PERSON_PHONE_NUMBER, personData);

            context.Registrations.Add(registration);
            await context.SaveChangesAsync();

            var handler = new GetRegistrationStatusQueryHandler(context, logger.Object);
            var query = new GetRegistrationStatusQuery(GUID);

            // Act 
            var resgistrationStatusResponse = await handler.Handle(query, CancellationToken.None);

            // Assert

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
            var passport = new Passport(SERIES, NUMBER, ISSUED_BY, ISSUE_DATE, ISSUE_ID, CITIZENSHIP);
            var personData = new PersonData(personName, passport, SNILS, FORM_DATA);

            return personData;
        }
    }
}