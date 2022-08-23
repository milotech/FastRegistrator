using FastRegistrator.ApplicationCore.Domain.Entities;

namespace FastRegistrator.ApplicationCore.DTOs.ICRegistrationDTOs
{
    public record class ICRegistrationData
    {
        public string PhoneNumber { get; init; } = null!;
        public PersonData PersonData { get; init; } = null!;

        public ICRegistrationData(string phoneNumber, PersonData personData)
        {
            PhoneNumber = phoneNumber;
            PersonData = personData;
        }
    }
}
