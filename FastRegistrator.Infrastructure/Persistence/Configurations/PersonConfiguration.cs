using FastRegistrator.ApplicationCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastRegistrator.Infrastructure.Persistence.Configurations
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder) 
        {
            builder.HasOne(p => p.PersonData)
                   .WithOne()
                   .HasForeignKey<PersonData>(p => p.Id);

            builder.HasMany(p => p.StatusHistory)
                   .WithOne()
                   .HasForeignKey("PersonId");

            builder.Ignore(p => p.PersonFormData);
        }
    }
}
