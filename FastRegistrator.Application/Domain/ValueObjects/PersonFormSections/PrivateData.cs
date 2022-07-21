namespace FastRegistrator.ApplicationCore.Domain.ValueObjects.PersonFormSections
{
    public class PrivateData
    {
        public DateTime? BirthDate { get; private set; }
        public string? PlaceOfBirth { get; private set; }
        public string? IdentityDocument { get; private set; }
        public Passport? Passport { get; private set; }
        public string? RegistryAddress { get; private set; }
        public string? ResidentiaAddress { get; private set; }
        public string? Phone { get; private set; }
        public string? Fax { get; private set; }
        public string? Email { get; private set; }

        public PrivateData(DateTime? birthDate, string? placeOfBirth, string? identityDocument, Passport? passport, string? registryAddress, string? residentiaAddress, string? phone, string? fax, string? email)
        {
            BirthDate = birthDate;
            PlaceOfBirth = placeOfBirth;
            IdentityDocument = identityDocument;
            Passport = passport;
            RegistryAddress = registryAddress;
            ResidentiaAddress = residentiaAddress;
            Phone = phone;
            Fax = fax;
            Email = email;
        }
    }
}
