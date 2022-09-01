using FastRegistrator.Application.Commands.CompleteRegistration;
using FastRegistrator.Application.Domain.Entities;
using FastRegistrator.Application.Domain.Enums;
using FastRegistrator.Application.Domain.ValueObjects;
using FastRegistrator.Application.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.ComponentModel;
using static FastRegistrator.UnitTests.Constants;

namespace FastRegistrator.UnitTests.Commands
{
    public class StopRegistrationWithErrorCommandHandlerTest : TestWithDbContext
    {
        [Fact]
        [Description("Arrange Registration data doesn't exist in database" +
                     "Act Handler for CompleteRegistrationWithErrorCommand is called" +
                     "Assert Handler throws a NotFoundException")]
        public async Task Handle_RegistrationDataDoesntExist_ThrowsNotFoundException()
        {
            // Arrange
            using var context = CreateDbContext();
            var logger = new Mock<ILogger<StopRegistrationWithErrorCommandHandler>>();
            IRequestHandler<CompleteRegistrationWithErrorCommand> handler = new StopRegistrationWithErrorCommandHandler(context, logger.Object);

            var command = new CompleteRegistrationWithErrorCommand(GUID, "Some error.", ErrorSource.FastRegistrator);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        [Description("Arrange Registration data exists in database" +
                     "Act Handler for CompleteRegistrationWithErrorCommand is called" +
                     "Assert Handler set Error status for registration")]
        public async Task Handle_RegistrationDataExists_CompleteRegistrationWithError()
        {
            // Arrange
            var personData = ConstructPersonData();
            var registration = new Registration(GUID, PHONE_NUMBER, personData);

            using var context = CreateDbContext();
            var entityEntry = context.Registrations.Add(registration);
            await context.SaveChangesAsync();

            var logger = new Mock<ILogger<StopRegistrationWithErrorCommandHandler>>();
            IRequestHandler<CompleteRegistrationWithErrorCommand> handler = new StopRegistrationWithErrorCommandHandler(context, logger.Object);

            var errorSource = ErrorSource.FastRegistrator;
            var command = new CompleteRegistrationWithErrorCommand(GUID, "Some error.", errorSource);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var assertPerson = await context.Registrations
                                .Include(p => p.StatusHistory.OrderByDescending(shi => shi.StatusDT).Take(1))
                                .FirstOrDefaultAsync(p => p.PhoneNumber == PHONE_NUMBER);

            Assert.Contains(assertPerson!.StatusHistory, shi => shi.Status == RegistrationStatus.Error);
            Assert.True(assertPerson!.Error!.Source == errorSource);
        }

        private PersonData ConstructPersonData()
        {
            var personName = new PersonName(FIRST_NAME, MIDDLE_NAME, LAST_NAME);
            var personData = new PersonData(personName, PHONE_NUMBER, PASSPORT_NUMBER, BIRTHDAY, INN, FORM_DATA);

            return personData;
        }
    }
}
