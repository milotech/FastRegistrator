using FastRegistrator.Application.Commands.CheckPerson;
using FastRegistrator.Application.Domain.Entities;
using FastRegistrator.Application.Domain.Enums;
using FastRegistrator.Application.Domain.ValueObjects;
using FastRegistrator.Application.DTOs.PrizmaService;
using FastRegistrator.Application.Exceptions;
using FastRegistrator.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.ComponentModel;
using System.Net;
using static FastRegistrator.Application.Commands.CheckPerson.CheckPersonCommandHandler;
using static FastRegistrator.UnitTests.Constants;

namespace FastRegistrator.UnitTests.Commands
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
        [Description("Arrange PersonCheck method throws HttpRequestException when retry need for request error" +
                     "Act Handler for SendDataToICCommand is called" +
                     "Assert Handler throws RetryRequiredException")]
        public async Task Handle_RequestError_RetriesDurationDoesntExceedMaximum_ThrowsRetryRequiredException()
        {
            // Arrange
            const int CURRENT_RETRIES_DURATION = MAX_RETRIES_DURATIONS.REQUEST_ERROR - 1;
            const int LOCAL_TIME_OFFSET = 180;

            var logger = new Mock<ILogger<CheckPersonCommandHandler>>();

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

            var prizmaService = new Mock<IPrizmaService>();
            prizmaService.Setup(x => x.PersonCheck(It.IsAny<PersonCheckRequest>(), It.IsAny<CancellationToken>()))
                     .Throws<HttpRequestException>();

            IRequestHandler<CheckPersonCommand> handler = new CheckPersonCommandHandler(context, prizmaService.Object, logger.Object, dateTime.Object);

            var command = new CheckPersonCommand(GUID, FIRST_NAME, PASSPORT_NUMBER, INN, BIRTHDAY);

            // Act & Assert
            await Assert.ThrowsAsync<RetryRequiredException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        [Description("Arrange PersonCheck method throws HttpRequestException when retry doesn't need for request error" +
                     "Act Handler for SendDataToICCommand is called" +
                     "Assert Handler set Error status for registration")]
        public async Task Handle_RequestError_RetriesDurationExceedMaximum_CompleteRegistrationWithError()
        {
            // Arrange
            const int CURRENT_RETRIES_DURATION = MAX_RETRIES_DURATIONS.REQUEST_ERROR + 1;
            const int LOCAL_TIME_OFFSET = 180;

            var logger = new Mock<ILogger<CheckPersonCommandHandler>>();

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

            var prizmaService = new Mock<IPrizmaService>();
            prizmaService.Setup(x => x.PersonCheck(It.IsAny<PersonCheckRequest>(), It.IsAny<CancellationToken>()))
                         .Throws<HttpRequestException>();

            IRequestHandler<CheckPersonCommand> handler = new CheckPersonCommandHandler(context, prizmaService.Object, logger.Object, dateTime.Object);

            var command = new CheckPersonCommand(GUID, FIRST_NAME, PASSPORT_NUMBER, INN, BIRTHDAY);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var assertPerson = await context.Registrations
                                            .Include(p => p.StatusHistory.OrderByDescending(shi => shi.StatusDT).Take(1))
                                            .FirstOrDefaultAsync(p => p.PhoneNumber == PHONE_NUMBER);

            Assert.Contains(assertPerson!.StatusHistory, shi => shi.Status == RegistrationStatus.Error);
            Assert.True(assertPerson!.Error!.Source == ErrorSource.PrizmaService);
        }

        [Fact]
        [Description("Arrange PersonCheck method throws HttpRequestException when retry need for unavailable response" +
                     "Act Handler for SendDataToICCommand is called" +
                     "Assert Handler throws RetryRequiredException")]
        public async Task Handle_UnavailableResponse_RetriesDurationDoesntExceedMaximum_ThrowsRetryRequiredException()
        {
            // Arrange
            const int CURRENT_RETRIES_DURATION = MAX_RETRIES_DURATIONS.UNAVAILABLE_RESPONSE - 1;
            const int LOCAL_TIME_OFFSET = 180;

            var logger = new Mock<ILogger<CheckPersonCommandHandler>>();

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

            var prizmaService = new Mock<IPrizmaService>();
            var httpRequestException = new HttpRequestException(null, null, statusCode: HttpStatusCode.ServiceUnavailable);
            prizmaService.Setup(x => x.PersonCheck(It.IsAny<PersonCheckRequest>(), It.IsAny<CancellationToken>()))
                         .Throws(httpRequestException);

            IRequestHandler<CheckPersonCommand> handler = new CheckPersonCommandHandler(context, prizmaService.Object, logger.Object, dateTime.Object);

            var command = new CheckPersonCommand(GUID, FIRST_NAME, PASSPORT_NUMBER, INN, BIRTHDAY);

            // Act & Assert
            await Assert.ThrowsAsync<RetryRequiredException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        [Description("Arrange PersonCheck method throws HttpRequestException when retry doesn't need for unavailable response" +
                     "Act Handler for SendDataToICCommand is called" +
                     "Assert Handler set Error status for registration")]
        public async Task Handle_UnavailableResponse_RetriesDurationExceedMaximum_CompleteRegistrationWithError()
        {
            // Arrange
            const int CURRENT_RETRIES_DURATION = MAX_RETRIES_DURATIONS.UNAVAILABLE_RESPONSE + 1;
            const int LOCAL_TIME_OFFSET = 180;

            var logger = new Mock<ILogger<CheckPersonCommandHandler>>();

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

            var prizmaService = new Mock<IPrizmaService>();
            var httpRequestException = new HttpRequestException(null, null, statusCode: HttpStatusCode.ServiceUnavailable);
            prizmaService.Setup(x => x.PersonCheck(It.IsAny<PersonCheckRequest>(), It.IsAny<CancellationToken>()))
                         .Throws(httpRequestException);

            IRequestHandler<CheckPersonCommand> handler = new CheckPersonCommandHandler(context, prizmaService.Object, logger.Object, dateTime.Object);

            var command = new CheckPersonCommand(GUID, FIRST_NAME, PASSPORT_NUMBER, INN, BIRTHDAY);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var assertPerson = await context.Registrations
                                            .Include(p => p.StatusHistory.OrderByDescending(shi => shi.StatusDT).Take(1))
                                            .FirstOrDefaultAsync(p => p.PhoneNumber == PHONE_NUMBER);

            Assert.Contains(assertPerson!.StatusHistory, shi => shi.Status == RegistrationStatus.Error);
            Assert.True(assertPerson!.Error!.Source == ErrorSource.PrizmaService);
        }

        [Fact]
        [Description("Arrange PersonCheck method throws regular Exception" +
                     "Act Handler for SendDataToICCommand is called" +
                     "Assert Handler set Error status for registration")]
        public async Task Handle_PersonCheckThrowsRegularException_CompleteRegistrationWithError()
        {
            // Arrange
            var logger = new Mock<ILogger<CheckPersonCommandHandler>>();

            var personData = ConstructPersonData();
            var registration = new Registration(GUID, PHONE_NUMBER, personData);

            using var context = CreateDbContext();
            var entityEntry = context.Registrations.Add(registration);
            await context.SaveChangesAsync();

            var dateTime = new Mock<IDateTime>();

            var prizmaService = new Mock<IPrizmaService>();
            prizmaService.Setup(x => x.PersonCheck(It.IsAny<PersonCheckRequest>(), It.IsAny<CancellationToken>()))
                         .Throws<Exception>();

            IRequestHandler<CheckPersonCommand> handler = new CheckPersonCommandHandler(context, prizmaService.Object, logger.Object, dateTime.Object);

            var command = new CheckPersonCommand(GUID, FIRST_NAME, PASSPORT_NUMBER, INN, BIRTHDAY);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var assertPerson = await context.Registrations
                                            .Include(p => p.StatusHistory.OrderByDescending(shi => shi.StatusDT).Take(1))
                                            .FirstOrDefaultAsync(p => p.PhoneNumber == PHONE_NUMBER);

            Assert.Contains(assertPerson!.StatusHistory, shi => shi.Status == RegistrationStatus.Error);
            Assert.True(assertPerson!.Error!.Source == ErrorSource.PrizmaService);
        }

        [Fact]
        [Description("Arrange PersonCheck method returns success PersonCheckResponse" +
                     "Act Handler for SendDataToICCommand is called" +
                     "Assert Handler sets PrizmaCheckResult")]
        public async Task Handle_PersonCheckReturnsSuccessResponse_SetPrizmaCheckResult()
        {
            // Arrange
            var logger = new Mock<ILogger<CheckPersonCommandHandler>>();

            var personData = ConstructPersonData();
            var registration = new Registration(GUID, PHONE_NUMBER, personData);

            using var context = CreateDbContext();
            var entityEntry = context.Registrations.Add(registration);
            await context.SaveChangesAsync();

            var dateTime = new Mock<IDateTime>();

            var prizmaService = new Mock<IPrizmaService>();
            var prizmaCheckResponse = new PersonCheckResponse
            {
                HttpStatusCode = (int)HttpStatusCode.OK,
                PersonCheckResult = new PersonCheckResult { RejectionReason = RejectionReason.None, PrizmaJsonResponse = "{}" }
            };
            prizmaService.Setup(x => x.PersonCheck(It.IsAny<PersonCheckRequest>(), It.IsAny<CancellationToken>()))
                         .Returns(Task.FromResult(prizmaCheckResponse));

            IRequestHandler<CheckPersonCommand> handler = new CheckPersonCommandHandler(context, prizmaService.Object, logger.Object, dateTime.Object);

            var command = new CheckPersonCommand(GUID, FIRST_NAME, PASSPORT_NUMBER, INN, BIRTHDAY);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var assertPerson = await context.Registrations
                                            .Include(p => p.StatusHistory.OrderByDescending(shi => shi.StatusDT).Take(1))
                                            .FirstOrDefaultAsync(p => p.PhoneNumber == PHONE_NUMBER);

            Assert.Contains(assertPerson!.StatusHistory, shi => shi.Status == RegistrationStatus.PrizmaCheckSuccessful);
        }

        [Fact]
        [Description("Arrange PersonCheck method returns failed PersonCheckResponse with errorResponse is null" +
                     "Act Handler for SendDataToICCommand is called" +
                     "Assert Handler set Error status for registration")]
        public async Task Handle_PersonCheckReturnsFailedResponseWithErrorResponseNull_CompleteRegistrationWithError()
        {
            // Arrange
            var logger = new Mock<ILogger<CheckPersonCommandHandler>>();

            var personData = ConstructPersonData();
            var registration = new Registration(GUID, PHONE_NUMBER, personData);

            using var context = CreateDbContext();
            var entityEntry = context.Registrations.Add(registration);
            await context.SaveChangesAsync();

            var dateTime = new Mock<IDateTime>();

            var prizmaService = new Mock<IPrizmaService>();
            var prizmaCheckResponse = new PersonCheckResponse
            {
                HttpStatusCode = (int)HttpStatusCode.BadRequest
            };
            prizmaService.Setup(x => x.PersonCheck(It.IsAny<PersonCheckRequest>(), It.IsAny<CancellationToken>()))
                         .Returns(Task.FromResult(prizmaCheckResponse));

            IRequestHandler<CheckPersonCommand> handler = new CheckPersonCommandHandler(context, prizmaService.Object, logger.Object, dateTime.Object);

            var command = new CheckPersonCommand(GUID, FIRST_NAME, PASSPORT_NUMBER, INN, BIRTHDAY);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var assertPerson = await context.Registrations
                                            .Include(p => p.StatusHistory.OrderByDescending(shi => shi.StatusDT).Take(1))
                                            .FirstOrDefaultAsync(p => p.PhoneNumber == PHONE_NUMBER);

            Assert.Contains(assertPerson!.StatusHistory, shi => shi.Status == RegistrationStatus.Error);
            Assert.True(assertPerson!.Error!.Source == ErrorSource.PrizmaService);
        }

        [Fact]
        [Description("Arrange PersonCheck method returns failed PersonCheckResponse and HttpStatusCode isn't ServiceUnavailable" +
                     "Act Handler for SendDataToICCommand is called" +
                     "Assert Handler set Error status for registration")]
        public async Task Handle_PersonCheckReturnsFailedResponseWithoutRetry_CompleteRegistrationWithError()
        {
            // Arrange
            var logger = new Mock<ILogger<CheckPersonCommandHandler>>();

            var personData = ConstructPersonData();
            var registration = new Registration(GUID, PHONE_NUMBER, personData);

            using var context = CreateDbContext();
            var entityEntry = context.Registrations.Add(registration);
            await context.SaveChangesAsync();

            var dateTime = new Mock<IDateTime>();

            var prizmaService = new Mock<IPrizmaService>();
            var prizmaCheckResponse = new PersonCheckResponse
            {
                HttpStatusCode = (int)HttpStatusCode.BadRequest,
                ErrorResponse = new PersonCheckError("Some error", 0, null)
            };
            prizmaService.Setup(x => x.PersonCheck(It.IsAny<PersonCheckRequest>(), It.IsAny<CancellationToken>()))
                         .Returns(Task.FromResult(prizmaCheckResponse));

            IRequestHandler<CheckPersonCommand> handler = new CheckPersonCommandHandler(context, prizmaService.Object, logger.Object, dateTime.Object);

            var command = new CheckPersonCommand(GUID, FIRST_NAME, PASSPORT_NUMBER, INN, BIRTHDAY);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var assertPerson = await context.Registrations
                                            .Include(p => p.StatusHistory.OrderByDescending(shi => shi.StatusDT).Take(1))
                                            .FirstOrDefaultAsync(p => p.PhoneNumber == PHONE_NUMBER);

            Assert.Contains(assertPerson!.StatusHistory, shi => shi.Status == RegistrationStatus.Error);
        }

        [Fact]
        [Description("Arrange PersonCheck method returns failed PersonCheckResponse and HttpStatusCode is ServiceUnavailable" +
                     "Act Handler for SendDataToICCommand is called" +
                     "Assert Handler throws RetryRequiredException")]
        public async Task Handle_PersonCheckReturnsFailedResponseWithRetry_ThrowsRetryRequiredException()
        {
            // Arrange
            const int CURRENT_RETRIES_DURATION = MAX_RETRIES_DURATIONS.UNAVAILABLE_RESPONSE - 1;
            const int LOCAL_TIME_OFFSET = 180;

            var logger = new Mock<ILogger<CheckPersonCommandHandler>>();

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

            var prizmaService = new Mock<IPrizmaService>();
            var prizmaCheckResponse = new PersonCheckResponse
            {
                HttpStatusCode = (int)HttpStatusCode.ServiceUnavailable,
                ErrorResponse = new PersonCheckError("Some error", 0, null)
            };
            prizmaService.Setup(x => x.PersonCheck(It.IsAny<PersonCheckRequest>(), It.IsAny<CancellationToken>()))
                         .Returns(Task.FromResult(prizmaCheckResponse));

            IRequestHandler<CheckPersonCommand> handler = new CheckPersonCommandHandler(context, prizmaService.Object, logger.Object, dateTime.Object);

            var command = new CheckPersonCommand(GUID, FIRST_NAME, PASSPORT_NUMBER, INN, BIRTHDAY);

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