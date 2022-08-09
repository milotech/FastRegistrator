namespace FastRegistrator.ApplicationCore.DTOs.GetStatusDTOs
{
    public enum ErrorSource
    {
        KonturPrizma,
        PrizmaService,
        FastRegistrator,
        IC
    }

    public class Error
    {
        public string Message { get; set; } = null!;
        public ErrorSource Source { get; set; }

        public Error(string message, ErrorSource source)
        {
            Message = message;
            Source = source;
        }
    }
}
