using FastRegistrator.ApplicationCore.Domain.Entities;
using FastRegistrator.ApplicationCore.DTOs.GetStatusDTOs;
using FastRegistrator.ApplicationCore.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FastRegistrator.ApplicationCore.Queries.GetStatus
{
    public record class GetStatusQuery : IRequest<GetStatusResponse>
    {
        public Guid Id { get; init; }
    }

    public class GetStatusQueryHandler : IRequestHandler<GetStatusQuery, GetStatusResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<GetStatusQueryHandler> _logger;

        public GetStatusQueryHandler(IApplicationDbContext context, ILogger<GetStatusQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<GetStatusResponse> Handle(GetStatusQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Trying to fetch a registration with Guid: {request.Id}");

            var query = _context.Registrations.Where(reg => reg.Id == request.Id);

            var registration = await query.FirstOrDefaultAsync(cancellationToken);

            StatusHistoryItem? statusHistoryItem = null;
            Exception? exception = null;

            if (registration is null)
            {
                exception = new Exception("There is no registration with such GUID.");
            }
            else 
            {
                statusHistoryItem = registration.StatusHistory.OrderByDescending(x => x.StatusDT).FirstOrDefault();
            }

            return new GetStatusResponse(statusHistoryItem, exception);
        }
    }
}