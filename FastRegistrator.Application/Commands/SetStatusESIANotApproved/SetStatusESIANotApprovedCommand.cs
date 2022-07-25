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
        public string Error { get; init; } = null!;
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
            var query = _dbContext.Persons.Where(p => p.PhoneNumber == request.PhoneNumber);

            var person = await query.FirstOrDefaultAsync(cancellationToken);

            if (person != null)
            {
                _logger.LogInformation($"Person with phone number '{request.PhoneNumber}' exists in database and not approved by ESIA");
                _logger.LogError(request.Error);
                person.SetESIANotApproved();
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                _logger.LogInformation($"Person with phone number '{request.PhoneNumber}' doesn't exist in database and not approved by ESIA");
                _logger.LogError(request.Error);
                var newPerson = new Person(request.PhoneNumber);
                newPerson.SetESIANotApproved();
                _dbContext.Persons.Add(newPerson);
                await _dbContext.SaveChangesAsync();
            }

            return Unit.Value;
        }
    }
}