using FastRegistrator.ApplicationCore.Domain.Enums;
using FastRegistrator.ApplicationCore.Domain.Events;

namespace FastRegistrator.ApplicationCore.Domain.Entities
{
    public class Registration : BaseEntity<Guid>
    {
        private List<StatusHistoryItem> _history = new();

        public string PhoneNumber { get; private set; } = null!;
        public PersonData PersonData { get; private set; } = null!;
        public PrizmaCheckResult? PrizmaCheckResult { get; private set; }
        public AccountData? AccountData { get; private set; }
        public Error? Error { get; private set; }
        public bool Completed { get; private set; }

        public IReadOnlyCollection<StatusHistoryItem> StatusHistory => _history;

        private Registration() { /* For EF */ }

        public Registration(Guid id, string phoneNumber, PersonData personData)
        {
            Id = id;
            PhoneNumber = phoneNumber;
            PersonData = personData;

            AddStatusToHistory(RegistrationStatus.PersonDataReceived);
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

            PrizmaCheckResult = prizmaCheckResult;

            if (!prizmaCheckResult.Result)
            {
                SetCompleted();
                AddStatusToHistory(RegistrationStatus.PrizmaCheckRejected);
            }
            else
            {
                AddDomainEvent(new PrizmaCheckPassedEvent(this));
                AddStatusToHistory(RegistrationStatus.PrizmaCheckSuccessful);
            }
        }

        public void SetError(Error error)
        {
            ValidateCompletion();
            SetCompleted();

            Error = error;

            AddStatusToHistory(RegistrationStatus.Error);
        }

        public void SetPersonDataSentToIC()
        {
            ValidateCompletion();
            AddStatusToHistory(RegistrationStatus.PersonDataSentToIC);
        }

        public void SetAccountOpened(/* some account data */)
        {
            ValidateCompletion();
            SetCompleted();

            AccountData = new AccountData();

            AddStatusToHistory(RegistrationStatus.AccountOpened);
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
