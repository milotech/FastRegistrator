using FastRegistrator.ApplicationCore.Domain;
using FastRegistrator.ApplicationCore.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FastRegistrator.Infrastructure.Persistence
{
    public static class MediatorExtensions
    {
        public static async Task<IEnumerable<INotification>> DispatchDomainEvents(this IMediator mediator, DbContext context)
        {
            var entities = context.ChangeTracker
                .Entries<BaseEntity>()
                .Where(e => e.Entity.DomainEvents != null && e.Entity.DomainEvents.Any())
                .Select(e => e.Entity);

            var domainEvents = entities
                .SelectMany(e => e.DomainEvents)
                .ToList();

            entities.ToList().ForEach(e => e.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent);
            }

            return domainEvents;
        }

        public static async Task DispatchCommittedDomainEvents(this IMediator mediator, IEnumerable<INotification> domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                Type committedEventType = typeof(CommittedEvent<>).MakeGenericType(domainEvent.GetType());
                var committedEvent = Activator.CreateInstance(committedEventType, domainEvent) as INotification;
                await mediator.Publish(committedEvent!);
            }
        }
    }
}
