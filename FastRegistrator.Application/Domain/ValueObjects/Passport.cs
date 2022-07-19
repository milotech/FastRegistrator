using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastRegistrator.ApplicationCore.Domain.ValueObjects
{
    public class Passport
    {
        public string Number { get; private set; }
        public string IssuedBy { get; private set; }
        public DateTime IssuedDate { get; private set; }

        public Passport(string number, string issuedBy, DateTime issuedDate)
        {
            Number = number;
            IssuedBy = issuedBy;
            IssuedDate = issuedDate;
        }
    }
}
