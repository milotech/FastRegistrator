using FastRegistrator.ApplicationCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastRegistrator.Infrastructure.Persistence.Configurations
{
    public class StatusHistoryItemConfiguration : IEntityTypeConfiguration<StatusHistoryItem>
    {
        public void Configure(EntityTypeBuilder<StatusHistoryItem> builder)
        {
            builder.HasOne(item => item.PrizmaCheckResponse!.PrizmaCheckResult)
                   .WithOne()
                   .HasForeignKey<PrizmaCheckResult>(p => p.Id);

            builder.HasOne(item => item.PrizmaCheckResponse!.PrizmaCheckError)
                   .WithOne()
                   .HasForeignKey<PrizmaCheckError>(p => p.Id);
        }
    }
}
