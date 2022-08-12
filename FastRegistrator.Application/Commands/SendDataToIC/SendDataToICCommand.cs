using FastRegistrator.ApplicationCore.Attributes;
using FastRegistrator.ApplicationCore.Domain.Entities;
using FastRegistrator.ApplicationCore.Domain.Enums;
using FastRegistrator.ApplicationCore.DTOs.RegistrationDTOs;
using FastRegistrator.ApplicationCore.Exceptions;
using FastRegistrator.ApplicationCore.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FastRegistrator.ApplicationCore.Commands.SendDataToIC
{
    [Command(CommandExecutionMode.InPlace)]
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

            try
            {
                var registration = await _dbContext.Registrations.Where(reg => reg.Id == request.Id)
                                                                 .Include(reg => reg.PersonData)
                                                                 .AsNoTracking()
                                                                 .FirstOrDefaultAsync(cancellationToken);

                if (registration is null)
                {
                    throw new NotFoundException($"There is no registration with Guid: {request.Id}");
                }

                var icRegistrationData = ConstructICRegistrationData(registration);

                var icRegistrationResponse = await _icService.SendData(icRegistrationData, cancellationToken);

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

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                
            }
        }

        private ICRegistrationData ConstructICRegistrationData(Registration registration)
            => new ICRegistrationData(registration.Id, registration.PhoneNumber, registration.PersonData);
    }
}
