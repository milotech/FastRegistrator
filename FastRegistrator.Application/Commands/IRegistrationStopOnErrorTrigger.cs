namespace FastRegistrator.ApplicationCore.Commands
{
    public interface IRegistrationStopOnErrorTrigger
    {
        Guid RegistrationId { get; }
    }
}
