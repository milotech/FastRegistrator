﻿using FastRegistrator.ApplicationCore.Domain.Enums;
using FastRegistrator.ApplicationCore.Domain.Events;
using FastRegistrator.ApplicationCore.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FastRegistrator.ApplicationCore.Startup
{
    public class RegistrationsRecoverer
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public RegistrationsRecoverer(IApplicationDbContext dbContext, IMediator mediator, ILogger<RegistrationsRecoverer> logger)
        {
            _dbContext = dbContext;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task RecoverIncompletedRegistrations(CancellationToken cancel)
        {
            _logger.LogInformation("Retrieve incompleted registrations from DB...");

            var registrations = await _dbContext.Registrations
                .Where(r => !r.Completed)
                .Include(r => r.StatusHistory.OrderByDescending(shi => shi.StatusDT).Take(1))
                .ToListAsync();

            _logger.LogInformation($"Retrieved {registrations.Count} incompleted registrations");

            foreach (var registration in registrations)
            {
                var status = registration.StatusHistory.First().Status;

                _logger.LogInformation($"Registration '{registration.Id}' status: {status}");

                switch (status)
                {
                    case RegistrationStatus.PrizmaCheckInProgress:
                        var regStartedEvent = new RegistrationStartedEvent(registration);
                        await _mediator.Publish(new CommittedEvent<RegistrationStartedEvent>(regStartedEvent));
                        break;
                    case RegistrationStatus.PrizmaCheckSuccessful:
                        var checkPassedEvent = new PrizmaCheckPassedEvent(registration);
                        await _mediator.Publish(new CommittedEvent<PrizmaCheckPassedEvent>(checkPassedEvent));
                        break;
                    default:
                        _logger.LogError($"Inconsistent db state for registration '{registration.Id}'");
                        break;
                }
            }
        }
    }
}