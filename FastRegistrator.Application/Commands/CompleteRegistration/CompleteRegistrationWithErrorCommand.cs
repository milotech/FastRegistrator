using FastRegistrator.Application.Attributes;
using FastRegistrator.Application.Domain.Entities;
using FastRegistrator.Application.Domain.Enums;
using FastRegistrator.Application.Exceptions;
using FastRegistrator.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FastRegistrator.Application.Commands.CompleteRegistration
{
    [Command(CommandExecutionMode.Parallel)]
    public record class CompleteRegistrationWithErrorCommand(
        Guid RegistrationId,
        string ErrorMessage,
        ErrorSource ErrorSource
    ) : IRequest;

    public class StopRegistrationWithErrorCommandHandler : AsyncRequestHandler<CompleteRegistrationWithErrorCommand>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger _logger;

        public StopRegistrationWithErrorCommandHandler(IApplicationDbContext dbContext, ILogger<StopRegistrationWithErrorCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        protected override async Task Handle(CompleteRegistrationWithErrorCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Registration '{request.RegistrationId}' should be completed with Error: ({request.ErrorSource}) {request.ErrorMessage}.");

            var registration = await _dbContext.Registrations.FirstOrDefaultAsync(r => r.Id == request.RegistrationId);

            if (registration is null)
            {
                throw new NotFoundException(nameof(Registration), request.RegistrationId);
            }

            var error = new Error(request.ErrorSource, request.ErrorMessage);
            registration.SetError(error);

            await _dbContext.SaveChangesAsync();
        }
    }
}
