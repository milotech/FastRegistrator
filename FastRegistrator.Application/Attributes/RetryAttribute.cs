namespace FastRegistrator.ApplicationCore.Attributes
{
    public class RetryAttribute: Attribute
    {
        public int MaxRetries { get; }

        public RetryAttribute(int maxRetriesCount)
        {
            MaxRetries = maxRetriesCount;
        }
    }
}
