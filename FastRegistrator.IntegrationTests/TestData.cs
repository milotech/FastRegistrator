using FastRegistrator.Application.Domain.Entities;
using FastRegistrator.Application.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastRegistrator.IntegrationTests
{
    public static class TestData
    {
        public static readonly Guid ExistentRegistrationId = new Guid("10000000-0000-0000-0000-000000000000");
        public static readonly Guid NonExistentRegistrationId = new Guid("20000000-0000-0000-0000-000000000000");

        public static readonly PersonName PersonName = new PersonName("Иван", "Иванович", "Иванов");
        public const string PhoneNumber = "+79001234567";
        public const string PassportNumber = "0000111222";

        public static PersonData CreatePersonData()
        {
            return new PersonData(PersonName, PhoneNumber, PassportNumber, null, null, "{}");            
        }

    }
}
