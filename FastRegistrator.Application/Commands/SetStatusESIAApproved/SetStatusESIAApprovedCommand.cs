using FastRegistrator.ApplicationCore.Domain.Entities;
using FastRegistrator.ApplicationCore.Domain.ValueObjects;
using FastRegistrator.ApplicationCore.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FastRegistrator.ApplicationCore.Commands.SetStatusESIAApproved
{
    public record class SetStatusESIAApprovedCommand : IRequest
    {
        public string PhoneNumber { get; init; } = null!;
        public string FirstName { get; init; } = null!;
        public string? MiddleName { get; init; }
        public string LastName { get; init; } = null!;
        public string Series { get; init; } = null!;
        public string Number { get; init; } = null!;
        public string IssuedBy { get; init; } = null!;
        public DateTime IssueDate { get; init; }
        public string IssueId { get; init; } = null!;
        public string Citizenship { get; init; } = null!;
        public string Snils { get; init; } = null!;
        public string FormData { get; init; } = null!;
    }

    public class SetStatusESIAApprovedCommandHandler : IRequestHandler<SetStatusESIAApprovedCommand>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly ILogger<SetStatusESIAApprovedCommandHandler> _logger;

        public SetStatusESIAApprovedCommandHandler(IApplicationDbContext dbContext, ILogger<SetStatusESIAApprovedCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Unit> Handle(SetStatusESIAApprovedCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Person with phone number '{request.PhoneNumber}' approved by ESIA.");

            var query = _dbContext.Persons.Where(p => p.PhoneNumber == request.PhoneNumber);

            var person = await query.FirstOrDefaultAsync(cancellationToken);

            if (person == null) 
            {
                _logger.LogInformation($"Person doesn't exist in database.");
                person = new Registration(request.PhoneNumber);
                _dbContext.Persons.Add(person);
            }

            var personData = ConstructPersonData(request);

            await _dbContext.SaveChangesAsync();

            return Unit.Value;
        }

        private PersonData ConstructPersonData(SetStatusESIAApprovedCommand request) 
        {
            var personName = new PersonName(request.FirstName, request.MiddleName, request.LastName);
            var passport = new Passport(request.Series, request.Number, request.IssuedBy, request.IssueDate, request.IssueId, request.Citizenship);
            var personData = new PersonData(personName, passport, request.Snils, request.FormData);

            return personData;
        }
    }
}
