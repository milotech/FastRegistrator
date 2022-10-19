using FastRegistrator.Application.Domain.Enums;
using FastRegistrator.Application.Domain.Events;
using FastRegistrator.Application.Exceptions;
using FastRegistrator.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FastRegistrator.Application.DomainEventHandlers
{
    public class RegistrationCompletedCommittedEventHandler : INotificationHandler<CommittedEvent<RegistrationCompletedEvent>>
    {
        private readonly ILogger _logger;
        private readonly IApplicationDbContext _dbContext;

        public RegistrationCompletedCommittedEventHandler(ILogger<RegistrationCompletedCommittedEventHandler> logger,
            IApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task Handle(CommittedEvent<RegistrationCompletedEvent> committedEvent, CancellationToken cancellationToken)
        {
            try
            {
                if (committedEvent.Event.Registration.StatusHistory.All(status => status.Status != RegistrationStatus.PersonDataSentToIC))
                    return;

                var registration = await _dbContext.Registrations.Where(reg => reg.Id == committedEvent.Event.Registration.Id)
                                                                 .Include(reg => reg.PersonData)
                                                                 .Include(r => r.StatusHistory.OrderByDescending(i => i.StatusDT).Take(1))
                                                                 .FirstOrDefaultAsync(cancellationToken);

                if (registration is null)
                    throw new NotFoundException(nameof(registration), committedEvent.Event.Registration.Id);

                registration.SetAccountOpened();

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, ex.Message);

                throw;
            }
        }
    }
}
