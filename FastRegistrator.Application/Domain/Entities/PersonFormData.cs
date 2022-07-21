using FastRegistrator.ApplicationCore.Domain.ValueObjects;

namespace FastRegistrator.ApplicationCore.Domain.Entities
{
    public class PersonFormData : BaseEntity
    {
        private List<BankDetail> _bankDetails = new();
        public IReadOnlyCollection<BankDetail> BankDetails => _bankDetails;
        public bool FinanceAgreement { get; }
        public string Sex { get; private set; } = null!;
        public string MoneySource { get; private set; } = null!;
        public string WorkPlace { get; private set; } = null!;
        public string JobTitle { get; private set; } = null!;
        public string PlannedOperations { get; private set; } = null!;
        public string PlannedForInvestmentMoney { get; private set; } = null!;
        public bool IsPresenceOfBeneficiary { get; private set; }
        public bool IsPoliticallyExposedPerson { get; private set; }
        public PrivateData? PrivateData { get; private set; }
        public bool IsTaxResidentRussia { get; private set; }
        public string Inn { get; private set; } = null!;
        public bool IsTaxResidentAnotherCountry { get; private set; }
        public string? FullName { get; private set; }
        public string? CountryOfTaxResidence { get; private set; }
        public string? Tin { get; private set; }
        public string? ReasonOfAbsenceTin { get; private set; }
        public string? OffshoreDetail { get; private set;}
        public USASectionDetails UsaSectionDetails { get; private set; }
    }
}
