namespace FastRegistrator.ApplicationCore.Domain.ValueObjects
{
    public class PersonName
    {
        public string FirstName { get; private set; }
        public string? MiddleName { get; private set; }
        public string LastName { get; private set; }

        public PersonName(string firstName, string? middleName, string lastName)
        {
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
        }
    }
}
