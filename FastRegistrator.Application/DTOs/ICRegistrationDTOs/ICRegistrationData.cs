using FastRegistrator.ApplicationCore.Domain.Entities;

namespace FastRegistrator.ApplicationCore.DTOs.ICRegistrationDTOs
{
    public record class ICRegistrationData
    {
        public Guid Id { get; init; }
        public string PhoneNumber { get; init; } = null!;
        public PersonData PersonData { get; init; } = null!;

        public ICRegistrationData(Guid id, string phoneNumber, PersonData personData)
        {
            Id = id;
            PhoneNumber = phoneNumber;
            PersonData = personData;
        }
    }
}
