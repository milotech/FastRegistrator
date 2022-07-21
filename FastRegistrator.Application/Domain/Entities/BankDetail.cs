namespace FastRegistrator.ApplicationCore.Domain.Entities
{
    public class BankDetail
    {
        public string Name { get; private set; }
        public string Account { get; private set; } = null!;
        public string Bik { get; private set; } = null!;
        public string Currency { get; private set; } = null!;

        public BankDetail(string name, string account, string bik, string currency)
        {
            Name = name;
            Account = account;
            Bik = bik;
            Currency = currency;
        }
    }
}
