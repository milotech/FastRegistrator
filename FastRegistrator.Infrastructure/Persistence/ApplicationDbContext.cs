using FastRegistrator.ApplicationCore.Domain.Entities;
using FastRegistrator.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FastRegistrator.Infrastructure.Persistence
{
    public partial class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext()
        { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Registration> Registrations => Set<Registration>();
        public DbSet<PersonData> PersonData => Set<PersonData>();
        public DbSet<StatusHistoryItem> StatusHistory => Set<StatusHistoryItem>();
        public DbSet<PrizmaCheckResult> PrizmaChecks => Set<PrizmaCheckResult>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
