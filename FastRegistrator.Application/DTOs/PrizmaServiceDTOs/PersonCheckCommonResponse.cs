namespace FastRegistrator.ApplicationCore.DTOs.PrizmaServiceDTOs
{
    public class PersonCheckCommonResponse
    {
        public int HttpStatusCode { get; set; }
        public PersonCheckResult? PersonCheckResult { get; set; }
        public ErrorResponse? ErrorResponse { get; set; }
    }
}
