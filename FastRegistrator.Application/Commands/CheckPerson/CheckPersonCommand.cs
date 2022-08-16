using FastRegistrator.ApplicationCore.Attributes;
using FastRegistrator.ApplicationCore.Domain.Entities;
using FastRegistrator.ApplicationCore.Domain.Enums;
using FastRegistrator.ApplicationCore.DTOs.PrizmaServiceDTOs;
using FastRegistrator.ApplicationCore.Exceptions;
using FastRegistrator.ApplicationCore.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

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

    public class CheckPersonCommandHandler: AsyncRequestHandler<CheckPersonCommand>
    {
        private IApplicationDbContext _dbContext;
        private IPrizmaService _prizmaService;
        private ILogger _logger;

        public CheckPersonCommandHandler(
            IApplicationDbContext dbContext,
            IPrizmaService prizmaService,
            ILogger<CheckPersonCommandHandler> logger
            )
        {
            _dbContext = dbContext;
            _prizmaService = prizmaService;
            _logger = logger;
        }

        protected override async Task Handle(CheckPersonCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Check Person '{command.Name}' for Registration '{command.RegistrationId}'");

            var prizmaRequest = new PersonCheckRequest
            {
                Fio = command.Name,
                PassportNumber = command.PassportNumber,
                DateOfBirth = command.BirthDt,
                Inn = command.INN
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
                _logger.LogError(ex, $"Failed to check Person for Registration '{registration.Id}'");

                var error = new Error(ErrorSource.FastRegistrator, ex.Message);
                registration.SetError(error);

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
                    if (IsRetryNeeded(errorResponse, registration))
                    {
                        throw new RetryRequiredException(errorResponse.Message);
                    }

                    Error error = ConstructErrorEntity(errorResponse, prizmaResponse.HttpStatusCode);
                    registration.SetError(error);
                }
            }
        }

        private bool IsRetryNeeded(ErrorResponse errorResponse, Registration registration)
        {
            return false;
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

            return new Error(source, message, JsonSerializer.Serialize(details));
        }
    }
}
