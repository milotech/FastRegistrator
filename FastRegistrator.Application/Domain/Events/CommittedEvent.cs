using MediatR;

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
