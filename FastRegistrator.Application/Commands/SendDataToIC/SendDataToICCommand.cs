using FastRegistrator.ApplicationCore.Attributes;
using FastRegistrator.ApplicationCore.Domain.Entities;
using FastRegistrator.ApplicationCore.Domain.Enums;
using FastRegistrator.ApplicationCore.DTOs.ICRegistrationDTOs;
using FastRegistrator.ApplicationCore.DTOs.RegistrationDTOs;
using FastRegistrator.ApplicationCore.Exceptions;
using FastRegistrator.ApplicationCore.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FastRegistrator.ApplicationCore.Commands.SendDataToIC
{
    [Command(CommandExecutionMode.Parallel)]
    public record class SendDataToICCommand(Guid Id) : IRequest;

    public class SendDataToICCommandHandler : AsyncRequestHandler<SendDataToICCommand>
    {
        private readonly IICService _icService;
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<SendDataToICCommandHandler> _logger;

        public SendDataToICCommandHandler(IICService icService, IApplicationDbContext dbContext, ILogger<SendDataToICCommandHandler> logger)
        {
            _icService = icService;
            _dbContext = dbContext;
            _logger = logger;
        }

        protected override async Task Handle(SendDataToICCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Trying to fetch a registration with Guid: {request.Id} for sending to IC");

            var registration = await _dbContext.Registrations.Where(reg => reg.Id == request.Id)
                                                             .Include(reg => reg.PersonData)
                                                             .AsNoTracking()
                                                             .FirstOrDefaultAsync(cancellationToken);
            
            if (registration is null)
            {
                throw new NotFoundException(nameof(registration), request.Id);
            }

            var icRegistrationData = ConstructICRegistrationData(registration);

            await SendDataToICAsync(registration, icRegistrationData, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task SendDataToICAsync(Registration registration, ICRegistrationData icRegistrationData, CancellationToken cancellationToken) 
        {
            ICRegistrationResponse? icRegistrationResponse;
            try
            {
                icRegistrationResponse = await _icService.SendData(icRegistrationData, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Failed to send person data with Giud: {registration.Id} to IC.");

                var error = new Error(ErrorSource.FastRegistrator, ex.Message, null);
                registration.SetError(error);

                return;
            }

            if (icRegistrationResponse.StatusCode == 200)
            {
                registration.SetPersonDataSentToIC();

                _logger.LogInformation("Person Data was sent to IC");
            }
            else
            {
                var error = new Error(ErrorSource.IC, icRegistrationResponse.ErrorMessage!, null);
                registration.SetError(error);

                _logger.LogInformation(icRegistrationResponse.ErrorMessage);
            }
        }

        private ICRegistrationData ConstructICRegistrationData(Registration registration)
            => new ICRegistrationData(registration.Id, registration.PhoneNumber, registration.PersonData);
    }
}
