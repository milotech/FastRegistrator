using FastRegistrator.ApplicationCore.Domain.Enums;

namespace FastRegistrator.ApplicationCore.Domain.Entities
{
    public class Person: BaseEntity
    {
        private List<StatusHistoryItem> _history = new ();

        public string PhoneNumber { get; private set; }
        public PersonData? PersonData { get; private set; }
        public PersonFormData? PersonFormData { get; private set; }

        public IReadOnlyCollection<StatusHistoryItem> StatusHistory => _history;

        public Person(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }

        public void SetESIANotApproved(PersonData data)
        {
            PersonData = data;

            AddStatusToHistory(PersonStatus.ESIANotApproved);
        }

        public void SetESIAApproved(PersonData data)
        {
            PersonData = data;

            AddStatusToHistory(PersonStatus.ESIAApproved);
        }

        public void SetPersonRejected(PersonData data, PersonFormData personFormData)
        {
            PersonData = data;
            PersonFormData = personFormData;

            AddStatusToHistory(PersonStatus.PersonRejected);
        }

        public void SetClientFilledApplication(PersonData data, PersonFormData personFormData) 
        {
            PersonData = data;
            PersonFormData = personFormData;

            AddStatusToHistory(PersonStatus.ClientFilledApplication);
        }

        public void SetPrizmaCheckInProgress()
            => AddStatusToHistory(PersonStatus.PrizmaCheckInProgress);
        

        public void SetPrizmaCheckResult(PrizmaCheckResult checkResult)
            => SetPrizmaFromCheckResult(checkResult);

        public void SetClientReadyForRegistration()
            => AddStatusToHistory(PersonStatus.ClientReadyForRegistration);
        

        public void SetAccountOpened()
            => AddStatusToHistory(PersonStatus.AccountOpened);
        

        public void SetAccountClosed()
            => AddStatusToHistory(PersonStatus.AccountClosed);
        

        private void SetPrizmaFromCheckResult(PrizmaCheckResult checkResult)
        {
            var statusHistoryItem = StatusHistoryItem.FromPrizmaCheckResult(checkResult);
            _history.Add(statusHistoryItem);
        }

        private void AddStatusToHistory(PersonStatus status) 
        {
            var statusHistoryItem = new StatusHistoryItem(status);
            _history.Add(statusHistoryItem);
        }
    }
}
