using FastRegistrator.ApplicationCore.Domain.ValueObjects;

namespace FastRegistrator.ApplicationCore.Domain.Entities
{
    public class PersonData : BaseEntity<Guid>
    {        
        public PersonName Name { get; private set; } = null!;
        public string PhoneNumber { get; private set; }
        public string PassportNumber { get; private set; } = null!;
        public DateTime? BirthDay { get; private set; }
        public string Inn { get; private set; } = null!;
        public string FormData { get; private set; } = null!;

        private PersonData() { /* For EF */ }

        public PersonData(PersonName name, string phoneNumber, string passportNumber, DateTime? birthDay, string inn, string formData)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            PassportNumber = passportNumber;
            BirthDay = birthDay;
            Inn = inn;
            FormData = formData;
        }
    }
}
