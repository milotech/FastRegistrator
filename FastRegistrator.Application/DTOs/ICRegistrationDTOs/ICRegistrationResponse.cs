namespace FastRegistrator.ApplicationCore.DTOs.ICRegistrationDTOs
{
    public record class ICRegistrationResponse
    {
        public string? ErrorMessage { get; init; }

        public ICRegistrationResponse(string? errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
