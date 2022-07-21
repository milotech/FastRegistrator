namespace FastRegistrator.ApplicationCore.Domain.Entities
{
    public class PersonFormData
    {
        private List<BankDetail> _bankDetails = new();
        public bool IsUserConsent { get; }
        public IReadOnlyCollection<BankDetail> BankDetails => _bankDetails;
        public bool Sex { get; private set; }
        public string SourceOriginFunds { get; private set; } = null!;
        public string PlaceOfWork { get; private set; } = null!;
        public string JobTitle { get; private set; } = null!;
        public string PlannedOperations { get; private set; } = null!;
        public string PlannedForInvestmentAmountOfFunds { get; private set; } = null!;
        public bool IsPresenceOfBeneficiary { get; private set; }
        public bool IsPresencePoliticallyExposedPersonStatus { get; private set; }
        public DateTime BirthDate { get; private set; }
        public string PlaceOfBirth { get; private set; } = null!;
        public string Citizenship { get; private set; } = null!;
        public string? IdentityDocument { get; private set; }
        public Passport? Passport { get; private set; }
        public string ResidenceAddress { get; private set; } = null!;
        public string ActualResidenceAddress { get; private set; } = null!;
        public string Phone { get; private set; } = null!;
        public string Fax { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public bool IsTaxResidentOfRF { get; private set; }
        public string Inn { get; private set; } = null!;
        public bool IsTaxResidentOfAnotherCountry { get; private set; }
        public string? FullName { get; private set; }
        public string? CountryOfTaxResidence { get; private set; }
        public string? Tin { get; private set; }
        public string? ReasonOfAbsenceTin { get; private set; }
        public string? OffshoreDetail { get; private set;}
        public bool IsCitizenshipUSA { get; private set; }
        public bool IsGreenCard { get; private set; }
        public bool IsLongTermStayUSA { get; private set; }
        public bool IsBornInUSAAndRefusalCitizenship { get; private set; }
        public bool IsResidentialInUSA { get; private set; }
        public bool IsPhoneNumberInUSA { get; private set; }
        public bool IsTaxResidentInUSA { get; private set; }
        public string? SSN { get; private set; }
        public string? ITIN { get; private set; }
        public string? FullNameIssuedByUSAAuthorities { get; private set; }
    }
}
