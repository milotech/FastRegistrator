namespace FastRegistrator.ApplicationCore.Domain.ValueObjects
{
    public class Passport
    {
        public string Serial { get; private set; }
        public string Number { get; private set; }
        public string IssuedBy { get; private set; }
        public DateTime IssuedDate { get; private set; }
        public string IssuerNumber { get; private set; }
        public string Citizenship { get; private set; }

        public Passport(string serial, string number, string issuedBy, DateTime issuedDate, string issuerNumber, string citizenship)
        {
            Serial = serial;
            Number = number;
            IssuedBy = issuedBy;
            IssuedDate = issuedDate;
            IssuerNumber = issuerNumber;
            Citizenship = citizenship;
        }
    }
}
