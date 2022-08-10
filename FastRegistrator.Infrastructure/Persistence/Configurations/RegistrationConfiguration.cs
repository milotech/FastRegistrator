using FastRegistrator.ApplicationCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastRegistrator.Infrastructure.Persistence.Configurations
{
    public class RegistrationConfiguration : IEntityTypeConfiguration<Registration>
    {
        public void Configure(EntityTypeBuilder<Registration> builder) 
        {
            builder.HasOne(p => p.PersonData)
                   .WithOne()
                   .HasForeignKey<PersonData>(p => p.Id);

            builder.HasMany(p => p.StatusHistory)
                   .WithOne()
                   .HasForeignKey("RegistrationId");

            builder.HasOne(item => item.PrizmaCheckResult)
                   .WithOne()
                   .HasForeignKey<PrizmaCheckResult>(p => p.Id);

            builder.HasOne(item => item.Error)
                   .WithOne()
                   .HasForeignKey<Error>(p => p.Id);

            builder.HasOne(item => item.AccountData)
                   .WithOne()
                   .HasForeignKey<AccountData>(p => p.Id);
        }
    }
}
