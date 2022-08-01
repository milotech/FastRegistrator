namespace FastRegistrator.Infrastructure.Options
{
    public class EventBusConnectionSettings
    {
        public string Host { get; set; } = null!;
        public string? VirtualHost { get; set; }
        public int Port { get; set; }
        public bool Ssl { get; set; }
        public string? User { get; set; }
        public string? Password { get; set; }
    }
}
