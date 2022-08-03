using FastRegistrator.ApplicationCore.Domain.ValueObjects;

namespace FastRegistrator.ApplicationCore.Domain.Entities
{
    public class PersonData : BaseEntity<Guid>
    {
        public PersonName Name { get; private set; } = null!;
        public Passport Passport { get; private set; } = null!;
        public string Snils { get; private set; } = null!;
        public string FormData { get; private set; } = null!;

        private PersonData() { /* For EF */ }

        public PersonData(PersonName name, Passport passport, string snils, string formData)
        {
            Name = name;
            Passport = passport;
            Snils = snils;
            FormData = formData;
        }
    }
}
