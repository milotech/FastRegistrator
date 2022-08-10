using FastRegistrator.ApplicationCore.Domain.Enums;

namespace FastRegistrator.ApplicationCore.DTOs.GetStatusDTOs
{
    public class RegistrationError
    {
        public string Message { get; set; } = null!;
        public ErrorSource Source { get; set; }

        public RegistrationError(string message, ErrorSource source)
        {
            Message = message;
            Source = source;
        }
    }
}
