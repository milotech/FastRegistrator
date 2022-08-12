namespace FastRegistrator.ApplicationCore.DTOs.ICRegistrationDTOs
{
    public record class ICRegistrationResponse
    {
        public int StatusCode { get; init; }
        public string? ErrorMessage { get; init; }
    }
}
