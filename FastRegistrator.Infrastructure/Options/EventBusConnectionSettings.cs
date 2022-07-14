using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastRegistrator.Infrastructure.Options
{
    public class EventBusConnectionSettings
    {
        public string Host { get; set; } = null!;
        public int Port { get; set; }
        public bool Ssl { get; set; }
        public string? User { get; set; }
        public string? Password { get; set; }
    }
}
