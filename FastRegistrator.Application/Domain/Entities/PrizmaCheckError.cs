namespace FastRegistrator.ApplicationCore.Domain.Entities
{
    public class PrizmaCheckError : BaseEntity<int>
    {
        public string ErrorMessage { get; private set; }
        public int StatusCode { get; private set; }
        public int PrizmaErrorCode { get; private set; }

        public PrizmaCheckError(string errorMessage, int statusCode, int prizmaErrorCode)
        {
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
            PrizmaErrorCode = prizmaErrorCode;
        }
    }
}
