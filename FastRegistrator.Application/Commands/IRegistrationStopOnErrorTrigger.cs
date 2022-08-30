namespace FastRegistrator.Application.Commands
{
    public interface IRegistrationStopOnErrorTrigger
    {
        Guid RegistrationId { get; }
    }
}
