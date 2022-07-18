using FastRegistrator.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastRegistrator.ApplicationCore.IntegrationEvents.Events
{
    public class TestIntegrationEvent : IIntegrationEvent
    {
        public int Value { get; set; }
        public string Description { get; set; } = null!;
    }
}
