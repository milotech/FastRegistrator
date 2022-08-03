using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastRegistrator.ApplicationCore.Domain.Events
{
    public sealed class CommittedEvent<T> : INotification
        where T : INotification
    {
        public T Event { get; }

        public CommittedEvent(T @event)
        {
            Event = @event;
        }
    }
}
