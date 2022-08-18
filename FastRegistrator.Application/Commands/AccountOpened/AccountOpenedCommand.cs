using FastRegistrator.ApplicationCore.Attributes;
using FastRegistrator.ApplicationCore.Domain.Entities;
using FastRegistrator.ApplicationCore.Domain.Enums;
using FastRegistrator.ApplicationCore.Exceptions;
using FastRegistrator.ApplicationCore.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FastRegistrator.ApplicationCore.Commands.AccountOpened
{
    [Command(CommandExecutionMode.InPlace)]
    public record class AccountOpenedCommand(Guid RegistrationId, string? ErrorMessage) : IRequest;

    public class AccountOpenedCommandHandler : AsyncRequestHandler<AccountOpenedCommand>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger _logger;

        public AccountOpenedCommandHandler(
            IApplicationDbContext dbContext,
            ILogger<AccountOpenedCommandHandler> logger
            )
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        protected override async Task Handle(AccountOpenedCommand command, CancellationToken cancellationToken)
        {
            var registration = await _dbContext.Registrations
                                               .Where(reg => reg.Id == command.RegistrationId)
                                               .Include(reg => reg.StatusHistory.OrderByDescending(shi => shi.StatusDT).Take(1))
                                               .FirstOrDefaultAsync();

            if (registration is null)
            { 
                throw new NotFoundException(nameof(Registration), command.RegistrationId);
            }

            if (command.ErrorMessage is null)
            {
                _logger.LogInformation($"Registration with Guid: {command.RegistrationId} is completed without errors.");

                registration.SetAccountOpened();
            }
            else
            {
                _logger.LogInformation($"Registration with Guid: {command.RegistrationId} is completed with error: {command.ErrorMessage}.");
                
                var error = new Error(ErrorSource.IC, command.ErrorMessage!);
                registration.SetError(error);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
