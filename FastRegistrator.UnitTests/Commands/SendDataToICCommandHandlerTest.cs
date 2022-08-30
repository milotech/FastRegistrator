using FastRegistrator.Application.Commands.SendDataToIC;
using FastRegistrator.Application.Domain.Entities;
using FastRegistrator.Application.Domain.Enums;
using FastRegistrator.Application.Domain.ValueObjects;
using FastRegistrator.Application.DTOs.ICService;
using FastRegistrator.Application.Exceptions;
using FastRegistrator.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.ComponentModel;
using static FastRegistrator.UnitTests.Constants;

namespace FastRegistrator.UnitTests.Commands
{
    public class SendDataToICCommandHandlerTest : TestWithDbContext
    {
        [Fact]
        [Description("Arrange Registration data doesn't exist in database" +
                     "Act Handler for SendDataToICCommand is called" +
                     "Assert Handler throws a NotFoundException")]
        public async Task Handle_RegistrationDataDoesntExist_ThrowsNotFoundException()
        {
            // Arrange
            var icService = new Mock<IICService>();
            var logger = new Mock<ILogger<SendDataToICCommandHandler>>();
            var dateTime = new Mock<IDateTime>();
            using var context = CreateDbContext();
            IRequestHandler<SendDataToICCommand> handler = new SendDataToICCommandHandler(icService.Object, context, logger.Object, dateTime.Object);

            var command = new SendDataToICCommand(GUID);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        [Description("Arrange ICService throws an exception after send data" +
                     "Act Handler for SendDataToICCommand is called" +
                     "Assert Save registration with error status and ErrorSource is FastRegistrator")]
        public async Task Handle_ICServiceThrowsException_SaveRegistrationWithErrorStatus()
        {
            // Arrange
            var logger = new Mock<ILogger<SendDataToICCommandHandler>>();
            var dateTime = new Mock<IDateTime>();
            using var context = CreateDbContext();

            var personData = ConstructPersonData();
            var registration = new Registration(GUID, PHONE_NUMBER, personData);

            var entityEntry = context.Registrations.Add(registration);
            await context.SaveChangesAsync();

            var icService = new Mock<IICService>();
            icService.Setup(x => x.SendData(It.IsAny<ICRegistrationData>(), It.IsAny<CancellationToken>()))
                     .Throws<Exception>();

            var command = new SendDataToICCommand(entityEntry.Entity.Id);

            IRequestHandler<SendDataToICCommand> handler = new SendDataToICCommandHandler(icService.Object, context, logger.Object, dateTime.Object);

            // Act
            var result = handler.Handle(command, CancellationToken.None);

            //Assert
            var assertPerson = await context.Registrations
                                            .Include(p => p.StatusHistory.OrderByDescending(shi => shi.StatusDT).Take(1))
                                            .FirstOrDefaultAsync(p => p.PhoneNumber == PHONE_NUMBER);

            Assert.Contains(assertPerson!.StatusHistory, shi => shi.Status == RegistrationStatus.Error);
            Assert.True(assertPerson!.Error!.Source == ErrorSource.IC);
        }

        [Fact]
        [Description("Arrange ICService return response without error message" +
                     "Act Handler for SendDataToICCommand is called" +
                     "Assert Save registration with status PersonDataSentToIC")]
        public async Task Handle_ICServiceReturnResponseWithoutErrorMessage_SaveRegistrationWithPersonDataSentToICStatus()
        {
            // Arrange
            const int successStatusCode = 200;
            var logger = new Mock<ILogger<SendDataToICCommandHandler>>();
            var dateTime = new Mock<IDateTime>();
            using var context = CreateDbContext();

            var personData = ConstructPersonData();
            var registration = new Registration(GUID, PHONE_NUMBER, personData);

            var entityEntry = context.Registrations.Add(registration);
            await context.SaveChangesAsync();

            var icService = new Mock<IICService>();
            var icRegistrationResponse = new ICRegistrationResponse(successStatusCode, null);
            icService.Setup(x => x.SendData(It.IsAny<ICRegistrationData>(), It.IsAny<CancellationToken>()))
                     .Returns(Task.FromResult(icRegistrationResponse));

            var command = new SendDataToICCommand(entityEntry.Entity.Id);

            IRequestHandler<SendDataToICCommand> handler = new SendDataToICCommandHandler(icService.Object, context, logger.Object, dateTime.Object);

            // Act
            var result = handler.Handle(command, CancellationToken.None);

            //Assert
            var assertPerson = await context.Registrations
                                            .Include(p => p.StatusHistory.OrderByDescending(shi => shi.StatusDT).Take(1))
                                            .FirstOrDefaultAsync(p => p.PhoneNumber == PHONE_NUMBER);

            Assert.Contains(assertPerson!.StatusHistory, shi => shi.Status == RegistrationStatus.PersonDataSentToIC);
        }

        [Fact]
        [Description("Arrange ICService return response with error message" +
                    "Act Handler for SendDataToICCommand is called" +
                    "Assert Save registration with error status and ErrorSource is IC")]
        public async Task Handle_ICServiceReturnResponseWithErrorMessage_SaveRegistrationWithErrorStatus()
        {
            // Arrange
            const int successStatusCode = 500;
            var logger = new Mock<ILogger<SendDataToICCommandHandler>>();
            var dateTime = new Mock<IDateTime>();
            using var context = CreateDbContext();

            var personData = ConstructPersonData();
            var registration = new Registration(GUID, PHONE_NUMBER, personData);

            var entityEntry = context.Registrations.Add(registration);
            await context.SaveChangesAsync();

            var icService = new Mock<IICService>();
            var icRegistrationError = new ICRegistrationError("Error message.", "Error detail.");
            var icRegistrationResponse = new ICRegistrationResponse(successStatusCode, icRegistrationError);
            icService.Setup(x => x.SendData(It.IsAny<ICRegistrationData>(), It.IsAny<CancellationToken>()))
                     .Returns(Task.FromResult(icRegistrationResponse));

            var command = new SendDataToICCommand(entityEntry.Entity.Id);

            IRequestHandler<SendDataToICCommand> handler = new SendDataToICCommandHandler(icService.Object, context, logger.Object, dateTime.Object);

            // Act
            var result = handler.Handle(command, CancellationToken.None);

            //Assert
            var assertPerson = await context.Registrations
                                            .Include(p => p.StatusHistory.OrderByDescending(shi => shi.StatusDT).Take(1))
                                            .FirstOrDefaultAsync(p => p.PhoneNumber == PHONE_NUMBER);

            Assert.Contains(assertPerson!.StatusHistory, shi => shi.Status == RegistrationStatus.Error);
            Assert.True(assertPerson!.Error!.Source == ErrorSource.IC);
        }

        private PersonData ConstructPersonData()
        {
            var personName = new PersonName(FIRST_NAME, MIDDLE_NAME, LAST_NAME);
            var personData = new PersonData(personName, PHONE_NUMBER, PASSPORT_NUMBER, BIRTHDAY, INN, FORM_DATA);

            return personData;
        }
    }
}
