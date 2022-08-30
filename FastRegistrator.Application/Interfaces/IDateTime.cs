namespace FastRegistrator.Application.Interfaces
{
    public interface IDateTime
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
        DateTime ServiceStarted { get; }
    }
}
