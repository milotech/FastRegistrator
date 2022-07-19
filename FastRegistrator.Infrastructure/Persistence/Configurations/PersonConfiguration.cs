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
                   .WithOne(p => p.Person)
                   .HasForeignKey<PersonData>(p => p.PersonId);
        }
    }
}
