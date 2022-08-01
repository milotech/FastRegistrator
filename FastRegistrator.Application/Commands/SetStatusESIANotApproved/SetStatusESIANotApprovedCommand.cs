using FastRegistrator.ApplicationCore.Domain.Entities;
using FastRegistrator.ApplicationCore.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FastRegistrator.ApplicationCore.Commands.SetStatusESIANotApproved
{
    public record class SetStatusESIANotApprovedCommand : IRequest
    {
        public string PhoneNumber { get; init; } = null!;
        public string? RejectReason { get; init; }
    }

    public class SetStatusESIANotApprovedCommandHandler : IRequestHandler<SetStatusESIANotApprovedCommand>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<SetStatusESIANotApprovedCommandHandler> _logger;

        public SetStatusESIANotApprovedCommandHandler(IApplicationDbContext dbContext, ILogger<SetStatusESIANotApprovedCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Unit> Handle(SetStatusESIANotApprovedCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Person with phone number '{request.PhoneNumber}' not approved by ESIA: {request.RejectReason}.");

            var query = _dbContext.Persons.Where(p => p.PhoneNumber == request.PhoneNumber);

            var person = await query.FirstOrDefaultAsync(cancellationToken);

            if (person == null)
            {
                _logger.LogInformation($"Person doesn't exist in database.");
                person = new Person(request.PhoneNumber);
                _dbContext.Persons.Add(person);
            }

            person.SetESIANotApproved();

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }
    }
}