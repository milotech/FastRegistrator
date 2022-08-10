using FastRegistrator.ApplicationCore.Domain.Enums;

namespace FastRegistrator.ApplicationCore.Domain.Entities
{
    public class Error : BaseEntity<Guid>
    {
        public ErrorSource Source { get; private set; }
        public string Message { get; private set; }
        public string? Details { get; private set; }

        public Error(ErrorSource source, string message, string? details)
        {
            Source = source;
            Message = message;
            Details = details;
        }
    }
}
