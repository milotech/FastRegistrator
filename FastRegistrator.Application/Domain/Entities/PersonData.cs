namespace FastRegistrator.ApplicationCore.Domain.Entities
{
    public class PersonData
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; } = new Person();
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? PassportNumber { get; set; }
        public DateTime? IssuedDate { get; set; }
        public DateTime? IssuedBy { get; set; }
        public string? Snils { get; set; }
    }
}
