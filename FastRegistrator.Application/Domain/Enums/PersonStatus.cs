namespace FastRegistrator.ApplicationCore.Domain.Enums
{
    public enum PersonStatus
    {
        ESIANotApproved,
        ESIAApproved,
        PersonRejected,
        SuccessRegistration = 0,
        FailedRegistration = 1,
        UserAlreadyExsist = 2,
        PrizmaCheckInProgress,
        PrizmaCheckRejected,
        PrizmaCheckSuccessful,
    }
}
