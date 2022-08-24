using FastRegistrator.ApplicationCore.Attributes;
using FastRegistrator.ApplicationCore.Domain.Entities;
using FastRegistrator.ApplicationCore.Domain.Enums;
using FastRegistrator.ApplicationCore.DTOs.PrizmaServiceDTOs;
using FastRegistrator.ApplicationCore.Exceptions;
using FastRegistrator.ApplicationCore.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text.Unicode;

namespace FastRegistrator.ApplicationCore.Commands.CheckPerson
{
    [Command(CommandExecutionMode.ExecutionQueue, ExecutionQueueParallelDegree = 10)]
    public record CheckPersonCommand(
        Guid RegistrationId,
        string Name,
        string PassportNumber,
        string? INN,
        DateTime? BirthDt
    ) : IRequest, IRegistrationStopOnErrorTrigger
    {
        public override string ToString()
        {
            return nameof(CheckPersonCommand) + $" {{ RegistrationId = {RegistrationId}, Name = {Name} }}";
        }
    }

    public class CheckPersonCommandHandler : AsyncRequestHandler<CheckPersonCommand>
    {
        // Maximum retries duration in minutes depending on the error type
        private static class RETRIES_DURATIONS
        {
            public const int REQUEST_ERROR = 10;
            public const int UNAVAILABLE_RESPONSE = 30;
        }

        private static JsonSerializerOptions _jsonOptions = new()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly IApplicationDbContext _dbContext;
        private readonly IPrizmaService _prizmaService;
        private readonly ILogger _logger;
        private readonly IDateTime _dateTime;

        public CheckPersonCommandHandler(
            IApplicationDbContext dbContext,
            IPrizmaService prizmaService,
            ILogger<CheckPersonCommandHandler> logger,
            IDateTime dateTime
            )
        {
            _dbContext = dbContext;
            _prizmaService = prizmaService;
            _logger = logger;
            _dateTime = dateTime;
        }

        protected override async Task Handle(CheckPersonCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Check Person '{command.Name}' for Registration '{command.RegistrationId}'");

            string passportNumber = Regex.Replace(command.PassportNumber, @"[+()\s\-]", string.Empty);
            string? inn = command.INN is not null ? Regex.Replace(command.INN, @"[+()\s\-]", string.Empty) : null;

            var prizmaRequest = new PersonCheckRequest
            {
                Fio = command.Name,
                PassportNumber = passportNumber,
                DateOfBirth = command.BirthDt,
                Inn = inn
            };

            var registration = await _dbContext.Registrations
                .Where(r => r.Id == command.RegistrationId)
                .Include(r => r.StatusHistory.OrderByDescending(i => i.StatusDT).Take(1))
                .FirstOrDefaultAsync();

            if (registration is null)
                throw new NotFoundException(nameof(Registration), command.RegistrationId);

            await TryCheckPerson(prizmaRequest, registration, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task TryCheckPerson(PersonCheckRequest request, Registration registration, CancellationToken cancellationToken)
        {
            PersonCheckCommonResponse? prizmaResponse;
            try
            {
                prizmaResponse = await _prizmaService.PersonCheck(request, cancellationToken);
            }
            catch (Exception ex)
            {
                if (IsRetryNeeded(ex, registration))
                    throw new RetryRequiredException(ex.Message);

                SetError(ex, registration);
                return;
            }

            if (prizmaResponse.PersonCheckResult != null)
            {
                var checkResultEntity = new PrizmaCheckResult(
                    rejectionReasonCode: prizmaResponse.PersonCheckResult.RejectionReason,
                    prizmaResponse: prizmaResponse.PersonCheckResult.PrizmaJsonResponse
                );

                registration.SetPrizmaCheckResult(checkResultEntity);
            }
            else
            {
                var errorResponse = prizmaResponse.ErrorResponse;

                if (errorResponse is null)
                {
                    Error error = new Error(ErrorSource.PrizmaService, "Invalid response from PrizmaService");
                    registration.SetError(error);
                }
                else
                {
                    if (IsRetryNeeded(prizmaResponse.HttpStatusCode, registration))
                        throw new RetryRequiredException(errorResponse.Message);

                    Error error = ConstructErrorEntity(errorResponse, prizmaResponse.HttpStatusCode);
                    registration.SetError(error);
                }
            }
        }

        private void SetError(Exception ex, Registration registration)
        {
            _logger.LogError(ex, $"Failed to check Person for Registration '{registration.Id}'");

            var error = new Error(ErrorSource.PrizmaService, ex.Message);
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

        private Error ConstructErrorEntity(ErrorResponse errorResponse, int httpResponseStatusCode)
        {
            var source = errorResponse.PrizmaErrorCode > 0
                ? ErrorSource.KonturPrizmaAPI
                : ErrorSource.PrizmaService;

            var message = errorResponse.Message;
            var details = new
            {
                HttpStatusCode = httpResponseStatusCode,
                PrizmaErrorCode = errorResponse.PrizmaErrorCode,
                Errors = errorResponse.Errors
            };

            return new Error(source, message, JsonSerializer.Serialize(details, _jsonOptions));
        }
    }
}
