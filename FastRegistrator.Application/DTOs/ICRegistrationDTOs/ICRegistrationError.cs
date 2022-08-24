namespace FastRegistrator.ApplicationCore.DTOs.ICRegistrationDTOs
{
    public class ICRegistrationError
    {
        public string Message { get; set; } = null!;
        public string? Detail { get; set; }

        public ICRegistrationError(string message, string? detail)
        {
            Message = message;
            Detail = detail;
        }
    }
}
