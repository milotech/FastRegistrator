using FastRegistrator.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastRegistrator.Infrastructure.Interfaces
{
    public interface IEventBus : IIntegrationEventsService
    {
        void Subscribe<T, TH>(int consumersCount = 1)
            where T : IIntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        void Unsubscribe<T, TH>()
            where T : IIntegrationEvent
            where TH : IIntegrationEventHandler<T>;
    }
}
