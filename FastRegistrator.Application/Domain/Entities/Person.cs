using FastRegistrator.ApplicationCore.Domain.Enums;

namespace FastRegistrator.ApplicationCore.Domain.Entities
{
    public class Person: BaseEntity
    {
        private List<StatusHistoryItem> _history = new ();

        public string PhoneNumber { get; private set; }
        public PersonData? PersonData { get; private set; }

        public IReadOnlyCollection<StatusHistoryItem> StatusHistory => _history;

        public Person(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }

        public void SetESIAApproved(PersonData data)
        {
            PersonData = data;

            var statusHistoryItem = new StatusHistoryItem(PersonStatus.ESIAApproved);
            _history.Add(statusHistoryItem);
        }

        public void SetPrizmaCheckResult(PrizmaCheckResult checkResult)
        {
            var historyItem = StatusHistoryItem.FromPrizmaCheckResult(checkResult);
            _history.Add(historyItem);
        }


    }
}
