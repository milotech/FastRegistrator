using FastRegistrator.ApplicationCore.Domain.Enums;

namespace FastRegistrator.ApplicationCore.DTOs.GetStatusDTOs
{
    public class RegistrationError
    {
        public ErrorSource Source { get; set; }
        public string Message { get; set; } = null!;
        public string? Details { get; set; }

        public RegistrationError(ErrorSource source, string message, string? details)
        {
            Source = source;
            Message = message;
            Details = details;
        }
    }
}
