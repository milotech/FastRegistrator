using FastRegistrator.ApplicationCore.Attributes;
using FastRegistrator.ApplicationCore.Domain.Entities;
using FastRegistrator.ApplicationCore.Domain.Enums;
using FastRegistrator.ApplicationCore.DTOs.ICService;
using FastRegistrator.ApplicationCore.Exceptions;
using FastRegistrator.ApplicationCore.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace FastRegistrator.ApplicationCore.Commands.SendDataToIC
{
    [Command(CommandExecutionMode.Parallel)]
    public record class SendDataToICCommand(Guid RegistrationId) : IRequest, IRegistrationStopOnErrorTrigger;

    public class SendDataToICCommandHandler : AsyncRequestHandler<SendDataToICCommand>
    {
        // Maximum retries duration in minutes depending on the error type
        private static class RETRIES_DURATIONS
        {
            public const int REQUEST_ERROR = 10;
            public const int UNAVAILABLE_RESPONSE = 30;
        }

        private readonly IICService _icService;
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<SendDataToICCommandHandler> _logger;
        private readonly IDateTime _dateTime;

        public SendDataToICCommandHandler(
            IICService icService, 
            IApplicationDbContext dbContext, 
            ILogger<SendDataToICCommandHandler> logger,
            IDateTime dateTime)
        {
            _icService = icService;
            _dbContext = dbContext;
            _logger = logger;
            _dateTime = dateTime;
        }

        protected override async Task Handle(SendDataToICCommand request, CancellationToken cancellationToken)
        {
            var registration = await _dbContext.Registrations.Where(reg => reg.Id == request.RegistrationId)
                                                             .Include(reg => reg.PersonData)
                                                             .Include(r => r.StatusHistory.OrderByDescending(i => i.StatusDT).Take(1))
                                                             .FirstOrDefaultAsync(cancellationToken);

            if (registration is null)
                throw new NotFoundException(nameof(registration), request.RegistrationId);

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
                if (IsRetryNeeded(ex, registration))
                    throw new RetryRequiredException(ex.Message);

                SetError(ex, registration);
                return;
            }

            if (icRegistrationResponse.ICRegistrationError is null)
            {
                registration.SetPersonDataSentToIC();

                _logger.LogInformation("Registration data was sent to IC");
            }
            else
            {
                if (IsRetryNeeded(icRegistrationResponse.HttpStatusCode, registration))
                    throw new RetryRequiredException(icRegistrationResponse.ICRegistrationError.Message);
                
                var error = new Error(ErrorSource.IC, icRegistrationResponse.ICRegistrationError.Message, icRegistrationResponse.ICRegistrationError.Detail);
                registration.SetError(error);
            }
        }

        private void SetError(Exception ex, Registration registration)
        {
            _logger.LogError(ex, $"Failed to send person data for Registration '{registration.Id}' to IC.");

            var error = new Error(ErrorSource.IC, ex.Message);
            registration.SetError(error);
        }

        private bool IsRetryNeeded(int httpStatusCode, Registration registration)
        {
            if (httpStatusCode == (int)HttpStatusCode.ServiceUnavailable)
                return CheckRetriesDuration(RETRIES_DURATIONS.UNAVAILABLE_RESPONSE, registration);

            return false;
        }

        private bool IsRetryNeeded(Exception exception, Registration registration)
        {
            if (exception is HttpRequestException requestException)
            {
                if (requestException.StatusCode == null)
                    return CheckRetriesDuration(RETRIES_DURATIONS.REQUEST_ERROR, registration);
                if (requestException.StatusCode == HttpStatusCode.ServiceUnavailable)
                    return CheckRetriesDuration(RETRIES_DURATIONS.UNAVAILABLE_RESPONSE, registration);
            }

            return false;
        }

        private bool CheckRetriesDuration(int maxDurationInMinutes, Registration registration)
        {
            var serviceStartedDt = _dateTime.ServiceStarted;
            var statusSetDt = registration.StatusHistory.FirstOrDefault()!.StatusDT;

            var thresholdDate = serviceStartedDt > statusSetDt ? serviceStartedDt : statusSetDt;

            return (_dateTime.Now - thresholdDate).TotalMinutes <= maxDurationInMinutes;
        }

        private ICRegistrationData ConstructICRegistrationData(Registration registration)
            => new ICRegistrationData(registration.PhoneNumber, registration.PersonData.FormData);
    }
}
