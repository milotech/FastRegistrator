using FastRegistrator.ApplicationCore.Domain.Entities;
using FastRegistrator.ApplicationCore.Domain.Enums;
using FastRegistrator.ApplicationCore.DTOs.GetStatusDTOs;
using FastRegistrator.ApplicationCore.Exceptions;
using FastRegistrator.ApplicationCore.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace FastRegistrator.ApplicationCore.Queries.GetStatus
{
    public record class GetRegistrationStatusQuery(Guid Id) : IRequest<RegistrationStatusResponse>;

    public class GetREgistrationStatusQueryHandler : IRequestHandler<GetRegistrationStatusQuery, RegistrationStatusResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<GetREgistrationStatusQueryHandler> _logger;

        public GetREgistrationStatusQueryHandler(IApplicationDbContext context, ILogger<GetREgistrationStatusQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<RegistrationStatusResponse> Handle(GetRegistrationStatusQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Trying to fetch a registration with Guid: {request.Id}");

            Expression<Func<Registration, IEnumerable<StatusHistoryItem>>> lastStatus =
                reg => reg.StatusHistory.OrderByDescending(s => s.StatusDT).Take(1);

            var registration = await _context.Registrations.Where(reg => reg.Id == request.Id)
                                                           .Include(lastStatus).ThenInclude(shi => shi.PrizmaCheckResult)
                                                           .Include(lastStatus).ThenInclude(shi => shi.PrizmaCheckError)
                                                           .AsNoTracking()
                                                           .FirstOrDefaultAsync(cancellationToken);

            if (registration is null)
            {
                throw new NotFoundException("There is no registration with such GUID.");
            }

            var statusHistoryItem = registration.StatusHistory.First();
            
            var prizmaRejectionReason = statusHistoryItem!.PrizmaCheckResult?.RejectionReasonCode;

            var error = ConstructError(statusHistoryItem);

            return new RegistrationStatusResponse(registration.Id, registration.Completed, statusHistoryItem.Status, prizmaRejectionReason, null, error);
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
                    message = statusHistoryItem.PrizmaCheckError.Message + "\n" + statusHistoryItem.PrizmaCheckError.Errors;
                }

                error = new Error(message, errorSource);
            }

            return error;
        }
    }
}