using FastRegistrator.Application.Domain.Enums;

namespace FastRegistrator.Application.Domain.Entities
{
    public class Error : BaseEntity<Guid>
    {
        public ErrorSource Source { get; private set; }
        public string Message { get; private set; }
        public string? Details { get; private set; }

        public Error(ErrorSource source, string message, string? details = null)
        {
            Source = source;
            Message = message;
            Details = details;
        }
    }
}
