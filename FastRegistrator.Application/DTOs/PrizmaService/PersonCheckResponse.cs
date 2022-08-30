namespace FastRegistrator.ApplicationCore.DTOs.PrizmaService
{
    public class PersonCheckResponse
    {
        public int HttpStatusCode { get; set; }
        public PersonCheckResult? PersonCheckResult { get; set; }
        public PersonCheckError? ErrorResponse { get; set; }
    }
}
