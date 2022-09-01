using FastRegistrator.Application.Interfaces;

namespace FastRegistrator.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
        public DateTime UtcNow => DateTime.UtcNow;
        public DateTime ServiceStarted => System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime();
    }
}
