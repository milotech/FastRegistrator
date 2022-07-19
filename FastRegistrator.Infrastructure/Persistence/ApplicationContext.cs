using FastRegistrator.ApplicationCore.Domain.Entities;
using FastRegistrator.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FastRegistrator.Infrastructure.Persistence
{
    public partial class ApplicationContext : DbContext, IApplicationDbContext
    {
        public ApplicationContext()
        { }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        { }

        public DbSet<Person> Persons => Set<Person>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            optionsBuilder.UseSqlServer("Data Source=");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
