using FastRegistrator.ApplicationCore.Domain.Enums;

namespace FastRegistrator.ApplicationCore.DTOs.RegistrationStatusDTOs
{
    public record class RegistrationError
    {
        public ErrorSource Source { get; init; }
        public string Message { get; init; } = null!;
        public string? Details { get; init; }

        public RegistrationError(ErrorSource source, string message, string? details)
        {
            Source = source;
            Message = message;
            Details = details;
        }
    }
}
