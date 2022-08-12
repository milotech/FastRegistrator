namespace FastRegistrator.ApplicationCore.Domain.ValueObjects
{
    public class Passport
    {
        public string Series { get; private set; }
        public string Number { get; private set; }
        public string IssuedBy { get; private set; }
        public DateTime IssueDate { get; private set; }
        public string IssueId { get; private set; }
        public string Citizenship { get; private set; }

        public Passport(string series, string number, string issuedBy, DateTime issueDate, string issueId, string citizenship)
        {
            Series = series;
            Number = number;
            IssuedBy = issuedBy;
            IssueDate = issueDate;
            IssueId = issueId;
            Citizenship = citizenship;
        }

        public override string ToString()
        {
            return Series + Number;
        }

        public static implicit operator string(Passport passport)
        {
            return passport.ToString();
        }
    }
}
