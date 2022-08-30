using FastRegistrator.Application.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastRegistrator.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Registration> Registrations { get; }
        DbSet<PersonData> PersonData { get; }
        DbSet<StatusHistoryItem> StatusHistory { get; }
        DbSet<PrizmaCheckResult> PrizmaChecks { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
