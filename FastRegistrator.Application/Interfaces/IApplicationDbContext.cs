using FastRegistrator.ApplicationCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastRegistrator.ApplicationCore.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Person> Persons { get; }
    }
}
