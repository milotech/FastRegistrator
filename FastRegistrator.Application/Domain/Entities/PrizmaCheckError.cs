namespace FastRegistrator.ApplicationCore.Domain.Entities
{
    public class PrizmaCheckError : BaseEntity<int>
    {
        public string Message { get; private set; }
        public string? Errors { get; private set; }
        public int StatusCode { get; private set; }
        public int PrizmaErrorCode { get; private set; }

        public PrizmaCheckError(string message, string? errors, int statusCode, int prizmaErrorCode)
        {
            Message = message;
            Errors = errors;
            StatusCode = statusCode;
            PrizmaErrorCode = prizmaErrorCode;
        }
    }
}
