﻿using FastRegistrator.Application.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastRegistrator.Infrastructure.Persistence.Configurations
{
    public class PersonDataConfiguration : IEntityTypeConfiguration<PersonData>
    {
        public void Configure(EntityTypeBuilder<PersonData> builder)
        {
            builder.OwnsOne(p => p.Name);
            builder.HasIndex(p => p.PhoneNumber);
        }
    }
}
