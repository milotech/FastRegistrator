using FastRegistrator.ApplicationCore.Domain.Entities;
using FastRegistrator.ApplicationCore.DTOs.GetStatusDTOs;
using FastRegistrator.ApplicationCore.Exceptions;
using FastRegistrator.ApplicationCore.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FastRegistrator.ApplicationCore.Queries.GetStatus
{
    public record class GetRegistrationStatusQuery(Guid Id) : IRequest<RegistrationStatusResponse>;

    public class GetRegistrationStatusQueryHandler : IRequestHandler<GetRegistrationStatusQuery, RegistrationStatusResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<GetRegistrationStatusQueryHandler> _logger;

        public GetRegistrationStatusQueryHandler(IApplicationDbContext context, ILogger<GetRegistrationStatusQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<RegistrationStatusResponse> Handle(GetRegistrationStatusQuery query, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Trying to fetch a registration with Guid: {query.Id}");

            var registration = await _context.Registrations.Where(reg => reg.Id == query.Id)
                                                           .Include(reg => reg.StatusHistory.OrderByDescending(shi => shi.StatusDT).Take(1))
                                                           .Include(reg => reg.PrizmaCheckResult)
                                                           .Include(reg => reg.AccountData)
                                                           .Include(reg => reg.Error)
                                                           .AsNoTracking()
                                                           .FirstOrDefaultAsync(cancellationToken);

            if (registration is null)
            {
                throw new NotFoundException(nameof(registration), query.Id);
            }

            var registrationStatus = registration.StatusHistory.First()!.Status;
            
            var prizmaRejectionReason = registration.PrizmaCheckResult?.RejectionReasonCode;

            var registrationError = ConstructRegistrationError(registration);

            return new RegistrationStatusResponse(registration.Id, registration.Completed, registrationStatus, prizmaRejectionReason, null, registrationError);
        }

        private RegistrationError? ConstructRegistrationError(Registration registration)
            => registration.Error is null
                            ? null
                            : new RegistrationError(registration.Error.Source, registration.Error.Message, registration.Error.Details);
    }
}