namespace FastRegistrator.ApplicationCore.Exceptions
{
    public enum EventBusError
    {
        Unknown,
        BrokerUnreachable,
        // To be continue...
    }

    public class EventBusException : Exception
    {
        public EventBusError Error { get; }

        public EventBusException(string message, EventBusError error, Exception? innerException = null)
            : base(message, innerException)
        {
            Error = error;
        }
    }
}
