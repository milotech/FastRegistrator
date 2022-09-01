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
using System.Net;
using static FastRegistrator.Application.Commands.SendDataToIC.SendDataToICCommandHandler;
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
            IRequestHandler<SendDataToICCommand> handler = new SendDataToICCommandHandler(context, icService.Object, logger.Object, dateTime.Object);

            var command = new SendDataToICCommand(GUID);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        [Description("Arrange SendData method throws HttpRequestException when retry need for request error" +
                     "Act Handler for SendDataToICCommand is called" +
                     "Assert Handler throws RetryRequiredException")]
        public async Task Handle_RequestError_RetriesDurationDoesntExceedMaximum_ThrowsRetryRequiredException()
        {
            // Arrange
            const int CURRENT_RETRIES_DURATION = MAX_RETRIES_DURATIONS.REQUEST_ERROR - 1;
            const int LOCAL_TIME_OFFSET = 180;

            var logger = new Mock<ILogger<SendDataToICCommandHandler>>();

            var personData = ConstructPersonData();
            var registration = new Registration(GUID, PHONE_NUMBER, personData);

            using var context = CreateDbContext();
            var entityEntry = context.Registrations.Add(registration);
            await context.SaveChangesAsync();

            var statusDt = context.Registrations.FirstOrDefault()!.StatusHistory.FirstOrDefault()!.StatusDT;

            var dateTime = new Mock<IDateTime>();
            dateTime.Setup(x => x.Now)
                    .Returns(statusDt.AddMinutes(CURRENT_RETRIES_DURATION + LOCAL_TIME_OFFSET));

            dateTime.Setup(x => x.UtcNow)
                    .Returns(statusDt.AddMinutes(CURRENT_RETRIES_DURATION));

            var icService = new Mock<IICService>();
            icService.Setup(x => x.SendData(It.IsAny<ICRegistrationData>(), It.IsAny<CancellationToken>()))
                     .Throws<HttpRequestException>();

            IRequestHandler<SendDataToICCommand> handler = new SendDataToICCommandHandler(context, icService.Object, logger.Object, dateTime.Object);

            var command = new SendDataToICCommand(GUID);

            // Act & Assert
            await Assert.ThrowsAsync<RetryRequiredException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        [Description("Arrange SendData method throws HttpRequestException when retry doesn't need for request error" +
                     "Act Handler for SendDataToICCommand is called" +
                     "Assert Handler set Error status for registration")]
        public async Task Handle_RequestError_RetriesDurationExceedsMaximum_CompleteRegistrationWithError()
        {
            // Arrange
            const int CURRENT_RETRIES_DURATION = MAX_RETRIES_DURATIONS.REQUEST_ERROR + 1;
            const int LOCAL_TIME_OFFSET = 180;

            var logger = new Mock<ILogger<SendDataToICCommandHandler>>();

            var personData = ConstructPersonData();
            var registration = new Registration(GUID, PHONE_NUMBER, personData);

            using var context = CreateDbContext();
            var entityEntry = context.Registrations.Add(registration);
            await context.SaveChangesAsync();

            var statusDt = context.Registrations.FirstOrDefault()!.StatusHistory.FirstOrDefault()!.StatusDT;

            var dateTime = new Mock<IDateTime>();
            dateTime.Setup(x => x.Now)
                    .Returns(statusDt.AddMinutes(CURRENT_RETRIES_DURATION + LOCAL_TIME_OFFSET));

            dateTime.Setup(x => x.UtcNow)
                    .Returns(statusDt.AddMinutes(CURRENT_RETRIES_DURATION));

            var icService = new Mock<IICService>();
            icService.Setup(x => x.SendData(It.IsAny<ICRegistrationData>(), It.IsAny<CancellationToken>()))
                     .Throws<HttpRequestException>();

            IRequestHandler<SendDataToICCommand> handler = new SendDataToICCommandHandler(context, icService.Object, logger.Object, dateTime.Object);

            var command = new SendDataToICCommand(GUID);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var assertPerson = await context.Registrations
                                            .Include(p => p.StatusHistory.OrderByDescending(shi => shi.StatusDT).Take(1))
                                            .FirstOrDefaultAsync(p => p.PhoneNumber == PHONE_NUMBER);

            Assert.Contains(assertPerson!.StatusHistory, shi => shi.Status == RegistrationStatus.Error);
            Assert.True(assertPerson!.Error!.Source == ErrorSource.IC);
        }

        [Fact]
        [Description("Arrange SendData method throws HttpRequestException when retry need for unavailable response" +
                     "Act Handler for SendDataToICCommand is called" +
                     "Assert Handler throws RetryRequiredException")]
        public async Task Handle_UnavailableResponse_RetriesDurationDoesntExceedMaximum_ThrowsRetryRequiredException()
        {
            // Arrange
            const int CURRENT_RETRIES_DURATION = MAX_RETRIES_DURATIONS.UNAVAILABLE_RESPONSE - 1;
            const int LOCAL_TIME_OFFSET = 180;

            var logger = new Mock<ILogger<SendDataToICCommandHandler>>();

            var personData = ConstructPersonData();
            var registration = new Registration(GUID, PHONE_NUMBER, personData);

            using var context = CreateDbContext();
            var entityEntry = context.Registrations.Add(registration);
            await context.SaveChangesAsync();

            var statusDt = context.Registrations.FirstOrDefault()!.StatusHistory.FirstOrDefault()!.StatusDT;

            var dateTime = new Mock<IDateTime>();
            dateTime.Setup(x => x.Now)
                    .Returns(statusDt.AddMinutes(CURRENT_RETRIES_DURATION + LOCAL_TIME_OFFSET));

            dateTime.Setup(x => x.UtcNow)
                    .Returns(statusDt.AddMinutes(CURRENT_RETRIES_DURATION));

            var icService = new Mock<IICService>();
            var httpRequestException = new HttpRequestException(null, null, statusCode: HttpStatusCode.ServiceUnavailable);
            icService.Setup(x => x.SendData(It.IsAny<ICRegistrationData>(), It.IsAny<CancellationToken>()))
                     .Throws(httpRequestException);

            IRequestHandler<SendDataToICCommand> handler = new SendDataToICCommandHandler(context, icService.Object, logger.Object, dateTime.Object);

            var command = new SendDataToICCommand(GUID);

            // Act & Assert
            await Assert.ThrowsAsync<RetryRequiredException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        [Description("Arrange SendData method throws HttpRequestException when retry doesn't need for unavailable response" +
                     "Act Handler for SendDataToICCommand is called" +
                     "Assert Handler set Error status for registration")]
        public async Task Handle_UnavailableResponse_RetriesDurationExceedsMaximum_CompleteRegistrationWithError()
        {
            // Arrange
            const int CURRENT_RETRIES_DURATION = MAX_RETRIES_DURATIONS.UNAVAILABLE_RESPONSE + 1;
            const int LOCAL_TIME_OFFSET = 180;

            var logger = new Mock<ILogger<SendDataToICCommandHandler>>();

            var personData = ConstructPersonData();
            var registration = new Registration(GUID, PHONE_NUMBER, personData);

            using var context = CreateDbContext();
            var entityEntry = context.Registrations.Add(registration);
            await context.SaveChangesAsync();

            var statusDt = context.Registrations.FirstOrDefault()!.StatusHistory.FirstOrDefault()!.StatusDT;

            var dateTime = new Mock<IDateTime>();
            dateTime.Setup(x => x.Now)
                    .Returns(statusDt.AddMinutes(CURRENT_RETRIES_DURATION + LOCAL_TIME_OFFSET));

            dateTime.Setup(x => x.UtcNow)
                    .Returns(statusDt.AddMinutes(CURRENT_RETRIES_DURATION));

            var icService = new Mock<IICService>();
            var httpRequestException = new HttpRequestException(null, null, statusCode: HttpStatusCode.ServiceUnavailable);
            icService.Setup(x => x.SendData(It.IsAny<ICRegistrationData>(), It.IsAny<CancellationToken>()))
                     .Throws(httpRequestException);

            IRequestHandler<SendDataToICCommand> handler = new SendDataToICCommandHandler(context, icService.Object, logger.Object, dateTime.Object);

            var command = new SendDataToICCommand(GUID);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var assertPerson = await context.Registrations
                                            .Include(p => p.StatusHistory.OrderByDescending(shi => shi.StatusDT).Take(1))
                                            .FirstOrDefaultAsync(p => p.PhoneNumber == PHONE_NUMBER);

            Assert.Contains(assertPerson!.StatusHistory, shi => shi.Status == RegistrationStatus.Error);
            Assert.True(assertPerson!.Error!.Source == ErrorSource.IC);
        }

        [Fact]
        [Description("Arrange SendData method throws regular Exception" +
                     "Act Handler for SendDataToICCommand is called" +
                     "Assert Handler set Error status for registration")]
        public async Task Handle_SendDataThrowsRegularException_CompleteRegistrationWithError()
        {
            // Arrange
            var logger = new Mock<ILogger<SendDataToICCommandHandler>>();

            var personData = ConstructPersonData();
            var registration = new Registration(GUID, PHONE_NUMBER, personData);

            using var context = CreateDbContext();
            var entityEntry = context.Registrations.Add(registration);
            await context.SaveChangesAsync();

            var dateTime = new Mock<IDateTime>();

            var icService = new Mock<IICService>();
            icService.Setup(x => x.SendData(It.IsAny<ICRegistrationData>(), It.IsAny<CancellationToken>()))
                     .Throws<Exception>();

            IRequestHandler<SendDataToICCommand> handler = new SendDataToICCommandHandler(context, icService.Object, logger.Object, dateTime.Object);

            var command = new SendDataToICCommand(GUID);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var assertPerson = await context.Registrations
                                            .Include(p => p.StatusHistory.OrderByDescending(shi => shi.StatusDT).Take(1))
                                            .FirstOrDefaultAsync(p => p.PhoneNumber == PHONE_NUMBER);

            Assert.Contains(assertPerson!.StatusHistory, shi => shi.Status == RegistrationStatus.Error);
            Assert.True(assertPerson!.Error!.Source == ErrorSource.IC);
        }

        [Fact]
        [Description("Arrange SendData method returns success ICRegistrationResponse" +
                     "Act Handler for SendDataToICCommand is called" +
                     "Assert Handler sets PersonDataSentToIC status")]
        public async Task Handle_SendDataReturnsSuccessResponse_SetStatusPersonDataSentToIC()
        {
            // Arrange
            var logger = new Mock<ILogger<SendDataToICCommandHandler>>();

            var personData = ConstructPersonData();
            var registration = new Registration(GUID, PHONE_NUMBER, personData);

            using var context = CreateDbContext();
            var entityEntry = context.Registrations.Add(registration);
            await context.SaveChangesAsync();

            var dateTime = new Mock<IDateTime>();

            var icService = new Mock<IICService>();
            var icRegistrationResponse = new ICRegistrationResponse((int)HttpStatusCode.OK, null);
            icService.Setup(x => x.SendData(It.IsAny<ICRegistrationData>(), It.IsAny<CancellationToken>()))
                     .Returns(Task.FromResult(icRegistrationResponse));

            IRequestHandler<SendDataToICCommand> handler = new SendDataToICCommandHandler(context, icService.Object, logger.Object, dateTime.Object);

            var command = new SendDataToICCommand(GUID);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var assertPerson = await context.Registrations
                                            .Include(p => p.StatusHistory.OrderByDescending(shi => shi.StatusDT).Take(1))
                                            .FirstOrDefaultAsync(p => p.PhoneNumber == PHONE_NUMBER);

            Assert.Contains(assertPerson!.StatusHistory, shi => shi.Status == RegistrationStatus.PersonDataSentToIC);
        }

        [Fact]
        [Description("Arrange SendData method returns failed ICRegistrationResponse and HttpStatusCode isn't ServiceUnavailable" +
                     "Act Handler for SendDataToICCommand is called" +
                     "Assert Handler set Error status for registration")]
        public async Task Handle_SendDataReturnsFailedResponseWithoutRetry_CompleteRegistrationWithError()
        {
            // Arrange
            var logger = new Mock<ILogger<SendDataToICCommandHandler>>();

            var personData = ConstructPersonData();
            var registration = new Registration(GUID, PHONE_NUMBER, personData);

            using var context = CreateDbContext();
            var entityEntry = context.Registrations.Add(registration);
            await context.SaveChangesAsync();

            var dateTime = new Mock<IDateTime>();

            var icService = new Mock<IICService>();
            var icRegistrationError = new ICRegistrationError("Some error.", null);
            var icRegistrationResponse = new ICRegistrationResponse((int)HttpStatusCode.BadRequest, icRegistrationError);
            icService.Setup(x => x.SendData(It.IsAny<ICRegistrationData>(), It.IsAny<CancellationToken>()))
                     .Returns(Task.FromResult(icRegistrationResponse));

            IRequestHandler<SendDataToICCommand> handler = new SendDataToICCommandHandler(context, icService.Object, logger.Object, dateTime.Object);

            var command = new SendDataToICCommand(GUID);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var assertPerson = await context.Registrations
                                            .Include(p => p.StatusHistory.OrderByDescending(shi => shi.StatusDT).Take(1))
                                            .FirstOrDefaultAsync(p => p.PhoneNumber == PHONE_NUMBER);

            Assert.Contains(assertPerson!.StatusHistory, shi => shi.Status == RegistrationStatus.Error);
            Assert.True(assertPerson!.Error!.Source == ErrorSource.IC);
        }

        [Fact]
        [Description("Arrange SendData method returns failed ICRegistrationResponse and HttpStatusCode is ServiceUnavailable" +
                     "Act Handler for SendDataToICCommand is called" +
                     "Assert Handler throws RetryRequiredException")]
        public async Task Handle_SendDataReturnsFailedResponseWithRetry_ThrowsRetryRequiredException()
        {
            // Arrange
            const int CURRENT_RETRIES_DURATION = MAX_RETRIES_DURATIONS.UNAVAILABLE_RESPONSE - 1;
            const int LOCAL_TIME_OFFSET = 180;

            var logger = new Mock<ILogger<SendDataToICCommandHandler>>();

            var personData = ConstructPersonData();
            var registration = new Registration(GUID, PHONE_NUMBER, personData);

            using var context = CreateDbContext();
            var entityEntry = context.Registrations.Add(registration);
            await context.SaveChangesAsync();

            var statusDt = context.Registrations.FirstOrDefault()!.StatusHistory.FirstOrDefault()!.StatusDT;

            var dateTime = new Mock<IDateTime>();
            dateTime.Setup(x => x.Now)
                    .Returns(statusDt.AddMinutes(CURRENT_RETRIES_DURATION + LOCAL_TIME_OFFSET));

            dateTime.Setup(x => x.UtcNow)
                    .Returns(statusDt.AddMinutes(CURRENT_RETRIES_DURATION));

            var icService = new Mock<IICService>();
            var icRegistrationError = new ICRegistrationError("Some error.", null);
            var icRegistrationResponse = new ICRegistrationResponse((int)HttpStatusCode.ServiceUnavailable, icRegistrationError);
            icService.Setup(x => x.SendData(It.IsAny<ICRegistrationData>(), It.IsAny<CancellationToken>()))
                     .Returns(Task.FromResult(icRegistrationResponse));

            IRequestHandler<SendDataToICCommand> handler = new SendDataToICCommandHandler(context, icService.Object, logger.Object, dateTime.Object);

            var command = new SendDataToICCommand(GUID);

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
