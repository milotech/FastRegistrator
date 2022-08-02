using FastRegistrator.ApplicationCore.Domain.Enums;

namespace FastRegistrator.ApplicationCore.Domain.Entities
{
    public class Registration : BaseEntity<Guid>
    {
        private List<StatusHistoryItem> _history = new();

        public string PhoneNumber { get; private set; }
        public PersonData? PersonData { get; private set; }
        public PersonFormData? PersonFormData { get; private set; }

        public IReadOnlyCollection<StatusHistoryItem> StatusHistory => _history;

        public Registration(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }

        public void SetESIAApproved(PersonData data)
        {
            PersonData = data;

            AddStatusToHistory(PersonStatus.ESIAApproved);
        }

        public void SetClientFilledApplication(PersonFormData personFormData) 
        {
            PersonFormData = personFormData;

            AddStatusToHistory(PersonStatus.ClientFilledApplication);
        }

        public void SetPrizmaCheckInProgress()
            => AddStatusToHistory(PersonStatus.PrizmaCheckInProgress);

        public void SetPrizmaCheckResult(PrizmaCheckResult checkResult)
        {
            var statusHistoryItem = StatusHistoryItem.FromPrizmaCheckResult(checkResult);
            _history.Add(statusHistoryItem);
        }

        public void SetClientReadyForRegistration()
            => AddStatusToHistory(PersonStatus.ClientReadyForRegistration);
        
        public void SetAccountOpened()
            => AddStatusToHistory(PersonStatus.AccountOpened);
        
        public void SetAccountClosed()
            => AddStatusToHistory(PersonStatus.AccountClosed);
        
        private void AddStatusToHistory(PersonStatus status) 
        {
            var statusHistoryItem = new StatusHistoryItem(status);
            _history.Add(statusHistoryItem);
        }
    }
}
