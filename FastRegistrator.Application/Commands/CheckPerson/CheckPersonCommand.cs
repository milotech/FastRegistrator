using FastRegistrator.ApplicationCore.Attributes;
using FastRegistrator.ApplicationCore.Domain.Entities;
using FastRegistrator.ApplicationCore.DTOs.PrizmaServiceDTOs;
using FastRegistrator.ApplicationCore.Exceptions;
using FastRegistrator.ApplicationCore.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastRegistrator.ApplicationCore.Commands.CheckPerson
{
    [Command(CommandExecutionMode.ExecutionQueue, ExecutionQueueParallelDegree = 10)]
    public record CheckPersonCommand(
        Guid RegistrationId,
        string Name,
        string PassportNumber,
        string? INN,
        DateTime? BirthDt
    ) : IRequest;

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
            var prizmaRequest = new PersonCheckRequest
            {
                Fio = command.Name,
                PassportNumber = command.PassportNumber,
                DateOfBirth = command.BirthDt,
                Inn = command.INN
            };

            try
            {
                var registration = await _dbContext.Registrations.FindAsync(command.RegistrationId);

                if (registration is null)
                    throw new NotFoundException(nameof(Registration), command.RegistrationId);

                var prizmaResponse = await _prizmaService.PersonCheck(prizmaRequest, cancellationToken);

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
                    // analyze prizmaResponse.ErrorResponse
                }
            }
            catch(Exception ex)
            {
                // later...
            }
        }
    }
}
