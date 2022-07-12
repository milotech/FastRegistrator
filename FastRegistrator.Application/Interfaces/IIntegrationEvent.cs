using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastRegistrator.ApplicationCore.Interfaces
{
    public interface IIntegrationEvent
    {
        // maybe Id, CreatedDate
    }

    public interface IIntegrationEventHandler<in TIntegrationEvent>
        where TIntegrationEvent : IIntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }
}
