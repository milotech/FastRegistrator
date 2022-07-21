namespace FastRegistrator.ApplicationCore.Domain.Entities
{
    public class BankDetail
    {
        public string CheckingAccount { get; private set; } = null!;
        public string BIC { get; private set; } = null!;
        public string Currency { get; private set; } = null!;

        public BankDetail(string checkingAccount, string bic, string currency)
        {
            CheckingAccount = checkingAccount;
            BIC = bic;
            Currency = currency;
        }
    }
}
