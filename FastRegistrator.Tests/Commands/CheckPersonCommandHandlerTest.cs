using FastRegistrator.ApplicationCore.Commands.CheckPerson;
using FastRegistrator.ApplicationCore.Domain.Entities;
using FastRegistrator.ApplicationCore.Domain.ValueObjects;
using FastRegistrator.ApplicationCore.DTOs.PrizmaServiceDTOs;
using FastRegistrator.ApplicationCore.Exceptions;
using FastRegistrator.ApplicationCore.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System.ComponentModel;
using static FastRegistrator.Tests.Constants;

namespace FastRegistrator.Tests.Commands
{
    public class CheckPersonCommandHandlerTest : TestWithDbContext
    {
        [Fact]
        [Description("Arrange Registration data doesn't exist in database" +
                     "Act Handler for SendDataToICCommand is called" +
                     "Assert Handler throws a NotFoundException")]
        public async Task Handle_RegistrationDataDoesntExist_ThrowsNotFoundException()
        {
            // Arrange
            using var context = CreateDbContext();
            var prizmaService = new Mock<IPrizmaService>();
            var logger = new Mock<ILogger<CheckPersonCommandHandler>>();
            var dateTime = new Mock<IDateTime>();
            IRequestHandler<CheckPersonCommand> handler = new CheckPersonCommandHandler(context, prizmaService.Object, logger.Object, dateTime.Object);

            var command = new CheckPersonCommand(GUID, FIRST_NAME, PASSPORT_NUMBER, INN, BIRTHDAY);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        [Description("Arrange PersonCheck method throws HttpRequestException when retry needed" +
                     "Act Handler for SendDataToICCommand is called" +
                     "Assert Handler throws RetryRequiredException")]
        public async Task Handle_PersonCheckThrowsHttpRequestExceptionWhenRetryNeeded_ThrowsRetryRequiredException()
        {
            // Arrange
            using var context = CreateDbContext();
            var logger = new Mock<ILogger<CheckPersonCommandHandler>>();

            var prizmaService = new Mock<IPrizmaService>();
            prizmaService.Setup(x => x.PersonCheck(It.IsAny<PersonCheckRequest>(), It.IsAny<CancellationToken>()))
                     .Throws<HttpRequestException>();

            var dateTime = new Mock<IDateTime>();
            dateTime.Setup(x => x.Now)
                    .Returns(DateTime.Now);

            dateTime.Setup(x => x.UtcNow)
                    .Returns(DateTime.UtcNow);

            IRequestHandler<CheckPersonCommand> handler = new CheckPersonCommandHandler(context, prizmaService.Object, logger.Object, dateTime.Object);

            var command = new CheckPersonCommand(GUID, FIRST_NAME, PASSPORT_NUMBER, INN, BIRTHDAY);

            var personData = ConstructPersonData();
            var registration = new Registration(GUID, PHONE_NUMBER, personData);

            var entityEntry = context.Registrations.Add(registration);
            await context.SaveChangesAsync();

            // Act & Assert
            await Assert.ThrowsAsync<RetryRequiredException>(() => handler.Handle(command, CancellationToken.None));
        }

        private PersonData ConstructPersonData()
        {
            var personName = new PersonName(FIRST_NAME, MIDDLE_NAME, LAST_NAME);
            var personData = new PersonData(personName, PHONE_NUMBER, PASSPORT_NUMBER, BIRTHDAY, INN, FORM_DATA);

            return personData;
        }
    }
}
