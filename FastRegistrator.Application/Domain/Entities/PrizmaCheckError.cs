namespace FastRegistrator.ApplicationCore.Domain.Entities
{
    public class PrizmaCheckError : BaseEntity<int>
    {
        public string? ErrorMessage { get; private set; }
        public int PrizmaErrorCode { get; private set; }
        public Dictionary<string, List<string>>? Errors { get; private set; }
    }
}
