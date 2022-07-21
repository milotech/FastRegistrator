using System;
using System.Data.Common;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using FastRegistrator.Infrastructure.Persistence;

namespace FastRegistrator.Tests
{
    public abstract class TestWithDbContext : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<ApplicationDbContext> _contextOptions;

        public TestWithDbContext()
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

        protected ApplicationDbContext CreateDbContext() => new ApplicationDbContext(_contextOptions);

        public void Dispose() => _connection.Dispose();
    }
}
