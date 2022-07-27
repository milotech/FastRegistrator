namespace FastRegistrator.ApplicationCore.Interfaces
{
    public interface IDateTime
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
    }

}
