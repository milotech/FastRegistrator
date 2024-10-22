﻿using FastRegistrator.Application.Attributes;
using FastRegistrator.Application.Domain.Entities;
using FastRegistrator.Application.Domain.Enums;
using FastRegistrator.Application.Exceptions;
using FastRegistrator.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FastRegistrator.Application.Commands.CompleteRegistration
{
    [Command(CommandExecutionMode.InPlace)]
    public record class CompleteRegistrationByICCommand(string PhoneNumber, string? ErrorMessage) : IRequest
    {
        public override string ToString()
        {
            string? errorProp = ErrorMessage is not null ? ", ErrorMessage = " + ErrorMessage : null;
            return nameof(CompleteRegistrationByICCommand) + $" {{ PhoneNumber = {PhoneNumber}{errorProp} }}";
        }
    }

    public class CompleteRegistrationByICCommandHandler : AsyncRequestHandler<CompleteRegistrationByICCommand>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger _logger;

        public CompleteRegistrationByICCommandHandler(
            IApplicationDbContext dbContext,
            ILogger<CompleteRegistrationByICCommandHandler> logger
            )
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        protected override async Task Handle(CompleteRegistrationByICCommand command, CancellationToken cancellationToken)
        {
            var registrations = await _dbContext.Registrations
                                               .Where(reg => !reg.Completed && reg.PersonData.PhoneNumber == command.PhoneNumber)
                                               .Include(reg => reg.StatusHistory.OrderBy(shi => shi.StatusDT).Take(1))
                                               .ToListAsync();

            Registration registration;

            if (registrations.Count == 0)
            {
                throw new NotFoundException(nameof(Registration), command.PhoneNumber);
            }
            else if (registrations.Count == 1)
            {
                registration = registrations[0];
            }
            else
            {
                _logger.LogWarning("Found multiple incompleted registrations with PhoneNumber=" + command.PhoneNumber);

                // take last one
                registration = registrations.OrderBy(r => r.StatusHistory.First().StatusDT).Last();
            }

            if (command.ErrorMessage is null)
            {
                _logger.LogInformation($"Registration for PhoneNumber '{command.PhoneNumber}' is completed without errors.");

                registration.SetAccountOpened();
            }
            else
            {
                _logger.LogInformation($"Registration for PhoneNumber '{command.PhoneNumber}' is completed with error: {command.ErrorMessage}.");

                var error = new Error(ErrorSource.IC, command.ErrorMessage!);
                registration.SetError(error);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
