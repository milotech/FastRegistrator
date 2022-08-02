using FastRegistrator.ApplicationCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastRegistrator.ApplicationCore.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Registration> Persons { get; }
        DbSet<PersonData> PersonData { get; }
        DbSet<StatusHistoryItem> StatusHistory { get; }
        DbSet<PrizmaCheckResult> PrizmaChecks { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
