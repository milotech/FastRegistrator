namespace FastRegistrator.Application.Exceptions
{
    public class RetryRequiredException : Exception
    {
        public RetryRequiredException(string message)
            : base(message)
        { }

        public RetryRequiredException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
