using FastRegistrator.Infrastructure.Persistence;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Data.Common;

namespace FastRegistrator.Tests
{
    public abstract class TestWithDbContext : IDisposable
    {
        private readonly DbConnection? _connection;
        private readonly DbContextOptions<ApplicationDbContext> _contextOptions;

        public TestWithDbContext(bool useSqlite = false)
        {
            if (useSqlite)
            {
                // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
                // at the end of the test (see Dispose below).
                _connection = new SqliteConnection("Filename=:memory:");
                _connection.Open();

                // These options will be used by the context instances in this test suite, including the connection opened above.
                _contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlite(_connection)
                    .Options;
            }
            else 
            {
                // These options will be used by the context instances in this test suite, including the connection opened above.
                _contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;
            }

            var mediator = new Mock<IMediator>().Object;

            // Create the schema
            using var context = new ApplicationDbContext(_contextOptions, mediator);
            context.Database.EnsureCreated();
        }

        protected ApplicationDbContext CreateDbContext() => new ApplicationDbContext(_contextOptions, new Mock<IMediator>().Object);

        public void Dispose() => _connection?.Dispose();
    }
}
