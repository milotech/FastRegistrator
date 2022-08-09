using FastRegistrator.ApplicationCore.Domain.Entities;
using FastRegistrator.ApplicationCore.DTOs.GetStatusDTOs;
using FastRegistrator.ApplicationCore.Exceptions;
using FastRegistrator.ApplicationCore.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FastRegistrator.ApplicationCore.Queries.GetStatus
{
    public record class GetStatusQuery(Guid Id) : IRequest<RegistrationStatusResponse>;

    public class GetStatusQueryHandler : IRequestHandler<GetStatusQuery, RegistrationStatusResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<GetStatusQueryHandler> _logger;

        public GetStatusQueryHandler(IApplicationDbContext context, ILogger<GetStatusQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<RegistrationStatusResponse> Handle(GetStatusQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Trying to fetch a registration with Guid: {request.Id}");

            var registration = await _context.Registrations.Where(reg => reg.Id == request.Id)
                                                           .Include(reg => reg.StatusHistory.OrderByDescending(shi => shi.StatusDT).Take(1))
                                                           .FirstOrDefaultAsync(cancellationToken);

            if (registration is null)
            {
                throw new NotFoundException("There is no registration with such GUID.");
            }

            var statusHistoryItem = registration.StatusHistory.First();
            
            var prizmaRejectionReason = statusHistoryItem!.PrizmaCheckResult!.RejectionReasonCode;

            var icResult = new ICResult();

            var error = ConstructError(statusHistoryItem);

            return new RegistrationStatusResponse(registration.Id, registration.Completed, statusHistoryItem.Status, prizmaRejectionReason, icResult, error);
        }

        private Error ConstructError(StatusHistoryItem statusHistoryItem) 
        {
            Error? error = null;

            if (statusHistoryItem.PrizmaCheckError is not null)
            {
                var message = string.Empty;
                var errorSource = statusHistoryItem.PrizmaCheckError.PrizmaErrorCode > 0
                                        ? ErrorSource.KonturPrizma
                                        : ErrorSource.PrizmaService;

                if (statusHistoryItem.PrizmaCheckError.Errors is not null)
                {
                    message = statusHistoryItem.PrizmaCheckError.Message + statusHistoryItem.PrizmaCheckError.Errors;
                }

                error = new Error(message, errorSource);
            }

            return error;
        }
    }
}