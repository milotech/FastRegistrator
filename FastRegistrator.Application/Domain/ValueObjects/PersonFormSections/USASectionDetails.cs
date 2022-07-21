namespace FastRegistrator.ApplicationCore.Domain.ValueObjects.PersonFormSections
{
    public class USASectionDetails
    {
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

        public USASectionDetails(bool isCitizenshipUSA, bool isGreenCard, bool isLongTermStayUSA, bool isBornInUSAAndRefusalCitizenship, bool isResidentialInUSA, bool isPhoneNumberInUSA, bool isTaxResidentInUSA, string? sSN, string? iTIN, string? fullNameIssuedByUSAAuthorities)
        {
            IsCitizenshipUSA = isCitizenshipUSA;
            IsGreenCard = isGreenCard;
            IsLongTermStayUSA = isLongTermStayUSA;
            IsBornInUSAAndRefusalCitizenship = isBornInUSAAndRefusalCitizenship;
            IsResidentialInUSA = isResidentialInUSA;
            IsPhoneNumberInUSA = isPhoneNumberInUSA;
            IsTaxResidentInUSA = isTaxResidentInUSA;
            SSN = sSN;
            ITIN = iTIN;
            FullNameIssuedByUSAAuthorities = fullNameIssuedByUSAAuthorities;
        }
    }
}
