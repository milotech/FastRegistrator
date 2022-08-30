namespace FastRegistrator.Application.DTOs.ICService
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
