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
        }
    }
}
