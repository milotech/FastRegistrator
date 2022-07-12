using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastRegistrator.ApplicationCore.Interfaces
{
    public interface IIntegrationEventsService
    {
        void Publish(IIntegrationEvent @event);
    }
}
