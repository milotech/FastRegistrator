using FastRegistrator.ApplicationCore.Domain.Enums;
using FastRegistrator.ApplicationCore.Domain.Events;

namespace FastRegistrator.ApplicationCore.Domain.Entities
{
    public class Registration : BaseEntity<Guid>
    {
        private List<StatusHistoryItem> _history = new();

        public string PhoneNumber { get; private set; } = null!;
        public PersonData PersonData { get; private set; } = null!;
        public bool Completed { get; private set; }

        public IReadOnlyCollection<StatusHistoryItem> StatusHistory => _history;

        private Registration() { /* For EF */ }

        public Registration(Guid id, string phoneNumber, PersonData personData)
        {
            Id = id;
            PhoneNumber = phoneNumber;
            PersonData = personData;

            AddStatusToHistory(RegistrationStatus.ClientFilledApplication);
            AddDomainEvent(new RegistrationStartedEvent(this));
        }

        public void SetPrizmaCheckInProgress()
        {
            ValidateCompletion();
            AddStatusToHistory(RegistrationStatus.PrizmaCheckInProgress);
        }

        public void SetPrizmaCheckResult(PrizmaCheckResult prizmaCheckResult)
        {
            ValidateCompletion();

            if(!prizmaCheckResult.Result)
            {
                SetCompleted();
            }
            else
            {
                AddDomainEvent(new PrizmaCheckPassedEvent(this));
            }

            var statusHistoryItem = StatusHistoryItem.FromPrizmaCheckResult(prizmaCheckResult);
            _history.Add(statusHistoryItem);
        }

        public void SetPrizmaCheckError(PrizmaCheckError prizmaCheckError)
        {
            ValidateCompletion();
            SetCompleted();

            var statusHistoryItem = StatusHistoryItem.FromPrizmaCheckError(prizmaCheckError);
            _history.Add(statusHistoryItem);
        }

        public void SetClientSentForRegistrationToIC()
        {
            ValidateCompletion();
            AddStatusToHistory(RegistrationStatus.ClientSentForRegistrationToIC);
        }

        public void SetAccountOpened()
        {
            ValidateCompletion();
            SetCompleted();
            AddStatusToHistory(RegistrationStatus.AccountOpened);
        }

        public void SetICRegistrationError()
        {
            ValidateCompletion();
            SetCompleted();
            AddStatusToHistory(RegistrationStatus.ICRegistrationFailed);
        }

        private void ValidateCompletion()
        {
            if (Completed)
            {
                throw new InvalidOperationException("Registration is completed");
            }
        }

        private void SetCompleted()
        {
            Completed = true;
            AddDomainEvent(new RegistrationCompletedEvent(this));
        }

        private void AddStatusToHistory(RegistrationStatus status) 
        {
            var statusHistoryItem = new StatusHistoryItem(status);
            _history.Add(statusHistoryItem);
        }
    }
}
