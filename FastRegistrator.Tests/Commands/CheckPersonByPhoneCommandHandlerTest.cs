using System;
using System.Data.Common;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using FastRegistrator.ApplicationCore.Interfaces;
using FastRegistrator.Infrastructure.Persistence;
using FastRegistrator.ApplicationCore.Domain.Entities;
using FastRegistrator.ApplicationCore.Domain.Enums;
using Moq;
using FastRegistrator.ApplicationCore.Commands.CheckPersonByPhone;
using Microsoft.Extensions.Logging;

namespace FastRegistrator.Tests.Commands
{
    public class CheckPersonByPhoneCommandHandlerTest : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<ApplicationDbContext> _contextOptions;

        #region ConstructorAndDispose
        public CheckPersonByPhoneCommandHandlerTest()
        {
            // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            // at the end of the test (see Dispose below).
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            _contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(_connection)
                .Options;

            // Create the schema
            using var context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();            
        }

        ApplicationDbContext CreateContext() => new ApplicationDbContext(_contextOptions);

        public void Dispose() => _connection.Dispose();
        #endregion

        [Fact]
        public async Task Handle_PrizmaRejectedOlderThanSixMonth_ReturnsPersonCanBeRegistered()
        {
            const string PERSON_PHONE_NUMBER = "+79157872577";
            const string PRIZMA_RESPONSE_JSON = "{}";

            // Arrange

            var logger = new Mock<ILogger<CheckPersonByPhoneCommandHandler>>();
            var dtService = new Mock<IDateTime>();
            dtService.Setup(dt => dt.Now).Returns(() => DateTime.Now.AddMonths(6).AddDays(1));
            dtService.Setup(dt => dt.UtcNow).Returns(() => DateTime.Now.AddMonths(6).AddDays(1));

            using ApplicationDbContext context = CreateContext();

            Person person = new Person(PERSON_PHONE_NUMBER);
            PrizmaCheckResult rejectedPrizmaCheck = new PrizmaCheckResult(RejectionReason.BlackListed, PRIZMA_RESPONSE_JSON);
            person.SetPrizmaCheckResult(rejectedPrizmaCheck);

            await context.SaveChangesAsync();

            // Act
            var handler = new CheckPersonByPhoneCommandHandler(context, dtService.Object, logger.Object);
            var result = await handler.Handle(new CheckPersonByPhoneCommand { PhoneNumber = PERSON_PHONE_NUMBER }, CancellationToken.None);

            // Assert
            Assert.True(result);
        }

        
    }
}
