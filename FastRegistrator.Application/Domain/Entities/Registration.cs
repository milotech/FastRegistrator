using FastRegistrator.ApplicationCore.Domain.Enums;
using FastRegistrator.ApplicationCore.Domain.Events;

namespace FastRegistrator.ApplicationCore.Domain.Entities
{
    public class Registration : BaseEntity<Guid>
    {
        private List<StatusHistoryItem> _history = new();

        public string PhoneNumber { get; private set; } = null!;
        public PersonData PersonData { get; private set; } = null!;

        public IReadOnlyCollection<StatusHistoryItem> StatusHistory => _history;

        private Registration() { /* For EF */ }

        public Registration(string phoneNumber, PersonData personData)
        {
            PhoneNumber = phoneNumber;
            PersonData = personData;

            AddStatusToHistory(RegistrationStatus.ClientFilledApplication);
            AddDomainEvent(new RegistrationStartedEvent(this));
        }

        public void SetPrizmaCheckInProgress()
            => AddStatusToHistory(RegistrationStatus.PrizmaCheckInProgress);

        public void SetPrizmaCheckResult(PrizmaCheckResult prizmaCheckResult)
        {
            var statusHistoryItem = StatusHistoryItem.FromPrizmaCheckResult(prizmaCheckResult);
            _history.Add(statusHistoryItem);
        }

        public void SetPrizmaCheckError(PrizmaCheckError prizmaCheckError)
        {
            var statusHistoryItem = StatusHistoryItem.FromPrizmaCheckError(prizmaCheckError);
            _history.Add(statusHistoryItem);
        }

        public void SetClientSentForRegistrationToIC()
            => AddStatusToHistory(RegistrationStatus.ClientSentForRegistrationToIC);
        
        public void SetAccountOpened()
            => AddStatusToHistory(RegistrationStatus.AccountOpened);
        
        public void SetAccountClosed()
            => AddStatusToHistory(RegistrationStatus.AccountClosed);
        
        private void AddStatusToHistory(RegistrationStatus status) 
        {
            var statusHistoryItem = new StatusHistoryItem(status);
            _history.Add(statusHistoryItem);
        }
    }
}
