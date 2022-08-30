using FastRegistrator.Application.Domain.Entities;
using FastRegistrator.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FastRegistrator.Infrastructure.Persistence
{
    public partial class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly IMediator _mediator;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator)
            : base(options)
        {
            _mediator = mediator;
        }

        public DbSet<Registration> Registrations => Set<Registration>();
        public DbSet<PersonData> PersonData => Set<PersonData>();
        public DbSet<StatusHistoryItem> StatusHistory => Set<StatusHistoryItem>();
        public DbSet<PrizmaCheckResult> PrizmaChecks => Set<PrizmaCheckResult>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var domainEvents = await _mediator.DispatchDomainEvents(this);

            var result = await base.SaveChangesAsync(cancellationToken);

            await _mediator.DispatchCommittedDomainEvents(domainEvents);

            return result;
        }
    }
}
