namespace FastRegistrator.ApplicationCore.Domain.Entities
{
    public class Person
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public PersonData? PersonData { get; set; }
    }
}
