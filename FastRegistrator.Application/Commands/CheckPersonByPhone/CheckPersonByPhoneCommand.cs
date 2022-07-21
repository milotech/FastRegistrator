using FastRegistrator.ApplicationCore.Domain.Enums;
using FastRegistrator.ApplicationCore.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FastRegistrator.ApplicationCore.Commands.CheckPersonByPhone
{
    public record class CheckPersonByPhoneCommand : IRequest<bool>
    {
        public string PhoneNumber { get; init; } = null!;
    }

    public class CheckPersonByPhoneCommandHandler : IRequestHandler<CheckPersonByPhoneCommand, bool>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IDateTime _dtService;
        private readonly ILogger<CheckPersonByPhoneCommandHandler> _logger;

        public CheckPersonByPhoneCommandHandler(
            IApplicationDbContext dbContext,
            IDateTime dtService,
            ILogger<CheckPersonByPhoneCommandHandler> logger
        ) 
        {
            _dbContext = dbContext;
            _dtService = dtService;
            _logger = logger;
        }

        public async Task<bool> Handle(CheckPersonByPhoneCommand request, CancellationToken cancellationToken) 
        {
            DateTime now = _dtService.UtcNow.Date;
            DateTime minDt = now.AddMonths(-6);

            var query = _dbContext.Persons
                .Where(p => p.PhoneNumber == request.PhoneNumber)
                .Select(p => new
                {
                    LastStatus = p.StatusHistory.OrderByDescending(shi => shi.StatusDT).FirstOrDefault(),
                    PrizmaRejected = p.StatusHistory.Any(shi =>
                        shi.Status == PersonStatus.PrizmaCheckRejected && shi.StatusDT >= minDt)
                });

            var checkResult = await query
                .FirstOrDefaultAsync(cancellationToken);

            if (checkResult != null)
            {
                if (checkResult.PrizmaRejected || checkResult.LastStatus?.Status == PersonStatus.AccountOpened)
                {
                    _logger.LogInformation($"Person with phone number '{request.PhoneNumber}' can be registered");
                    return false;
                }
            }

            _logger.LogInformation($"Person with phone number '{request.PhoneNumber}' can't be registered");
            return true;
        }
    }
}
