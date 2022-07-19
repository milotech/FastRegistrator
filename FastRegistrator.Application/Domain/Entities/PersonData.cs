using FastRegistrator.ApplicationCore.Domain.ValueObjects;

namespace FastRegistrator.ApplicationCore.Domain.Entities
{
    public class PersonData : BaseEntity
    {
        public PersonName Name { get; private set; }
        public Passport Passport { get; private set; }
        public string Snils { get; private set; }

        public PersonData(PersonName name, Passport passport, string snils)
        {
            Name = name;
            Passport = passport;
            Snils = snils;
        }
    }
}
