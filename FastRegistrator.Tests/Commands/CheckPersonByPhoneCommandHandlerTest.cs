using FastRegistrator.ApplicationCore.Commands.CheckPersonByPhone;
using FastRegistrator.ApplicationCore.Domain.Entities;
using FastRegistrator.ApplicationCore.Domain.Enums;
using FastRegistrator.ApplicationCore.Interfaces;
using FastRegistrator.Infrastructure.Persistence;
using Moq;
using Microsoft.Extensions.Logging;

namespace FastRegistrator.Tests.Commands
{
    public class CheckPersonByPhoneCommandHandlerTest : TestWithDbContext
    {
        [Fact]
        public async Task Handle_PersonDoesNotExist_ReturnsCanBeRegistered()
        {
            const string PERSON_PHONE_NUMBER = "+79157872577";

            // Arrange

            var logger = new Mock<ILogger<CheckPersonByPhoneCommandHandler>>();
            var dtService = GetDateTimeService();

            using ApplicationDbContext context = CreateDbContext();

            // Act
            var handler = new CheckPersonByPhoneCommandHandler(context, dtService, logger.Object);
            var result = await handler.Handle(new CheckPersonByPhoneCommand { PhoneNumber = PERSON_PHONE_NUMBER }, CancellationToken.None);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Handle_PrizmaRejected_MoreThanSixMonth_ReturnsCanBeRegistered()
        {
            const string PERSON_PHONE_NUMBER = "+79157872577";
            const string PRIZMA_RESPONSE_JSON = "{}";

            // Arrange

            var logger = new Mock<ILogger<CheckPersonByPhoneCommandHandler>>();
            var dtService = GetDateTimeService((dt) => dt.AddMonths(6).AddDays(1));

            using ApplicationDbContext context = CreateDbContext();

            Person person = new Person(PERSON_PHONE_NUMBER);
            PrizmaCheckResult rejectedPrizmaCheck = new PrizmaCheckResult(RejectionReason.BlackListed, PRIZMA_RESPONSE_JSON);
            person.SetPrizmaCheckResult(rejectedPrizmaCheck);
            context.Persons.Add(person);

            await context.SaveChangesAsync();

            // Act
            var handler = new CheckPersonByPhoneCommandHandler(context, dtService, logger.Object);
            var result = await handler.Handle(new CheckPersonByPhoneCommand { PhoneNumber = PERSON_PHONE_NUMBER }, CancellationToken.None);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Handle_PrizmaRejected_LessThanSixMonth_ReturnsCanNotBeRegistered()
        {
            const string PERSON_PHONE_NUMBER = "+79157872577";
            const string PRIZMA_RESPONSE_JSON = "{}";

            // Arrange

            var logger = new Mock<ILogger<CheckPersonByPhoneCommandHandler>>();
            var dtService = GetDateTimeService((dt) => dt.AddMonths(6).AddDays(-1));

            using ApplicationDbContext context = CreateDbContext();

            Person person = new Person(PERSON_PHONE_NUMBER);
            PrizmaCheckResult rejectedPrizmaCheck = new PrizmaCheckResult(RejectionReason.BlackListed, PRIZMA_RESPONSE_JSON);
            person.SetPrizmaCheckResult(rejectedPrizmaCheck);
            context.Persons.Add(person);

            await context.SaveChangesAsync();

            // Act
            var handler = new CheckPersonByPhoneCommandHandler(context, dtService, logger.Object);
            var result = await handler.Handle(new CheckPersonByPhoneCommand { PhoneNumber = PERSON_PHONE_NUMBER }, CancellationToken.None);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Handle_AccountOpened_ReturnsCanNotBeRegistered()
        {
            const string PERSON_PHONE_NUMBER = "+79157872577";

            // Arrange

            var logger = new Mock<ILogger<CheckPersonByPhoneCommandHandler>>();
            var dtService = GetDateTimeService((dt) => dt.AddMonths(6).AddDays(-1));

            using ApplicationDbContext context = CreateDbContext();

            Person person = new Person(PERSON_PHONE_NUMBER);
            person.SetAccountOpened();
            context.Persons.Add(person);

            await context.SaveChangesAsync();

            // Act
            var handler = new CheckPersonByPhoneCommandHandler(context, dtService, logger.Object);
            var result = await handler.Handle(new CheckPersonByPhoneCommand { PhoneNumber = PERSON_PHONE_NUMBER }, CancellationToken.None);

            // Assert
            Assert.False(result);
        }

        private IDateTime GetDateTimeService(Func<DateTime, DateTime>? adjustingDtFunc = null)
        {
            adjustingDtFunc = adjustingDtFunc ?? ((dt) => dt);

            var dtService = new Mock<IDateTime>();
            dtService.Setup(dt => dt.Now).Returns(() => adjustingDtFunc(DateTime.Now));
            dtService.Setup(dt => dt.UtcNow).Returns(() => adjustingDtFunc(DateTime.UtcNow));
            return dtService.Object;
        }
    }
}
