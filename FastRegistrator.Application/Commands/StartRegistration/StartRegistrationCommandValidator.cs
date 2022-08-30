using FastRegistrator.Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace FastRegistrator.Application.Commands.StartRegistration
{
    public class StartRegistrationCommandValidator : AbstractValidator<StartRegistrationCommand>
    {
        private readonly IApplicationDbContext _dbContext;

        public const string IdIsEmpty = "Registration Identifier is empty.";
        public const string RegistrationAlreadyExists = "Registration with specified Identifier already exists";
        public const string PhoneNumberIsEmpty = "Mobile phone is empty.";
        public const string PhoneNumberTooLong = "Mobile phone can contain up to 16 symbols.";
        public const string FirstNameIsEmpty = "First name is empty.";
        public const string LastNameIsEmpty = "Last name is empty.";
        public const string PassportNumberIsEmpty = "PassportNumber is empty.";
        public const string PassportNumberHasWrongFormat = "PassportNumber has wrong format.";
        public const string InnIsEmpty = "INN is empty.";
        public const string InnHasWrongFormat = "INN has wrong format.";
        public const string FormDataIsEmpty = "FormData is empty.";

        public StartRegistrationCommandValidator(IApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;

            RuleFor(command => command.RegistrationId)
                .NotEmpty().WithMessage(IdIsEmpty)
                .MustAsync(BeUniqueId).WithMessage(RegistrationAlreadyExists);

            Transform(command => command.PhoneNumber, RemoveAllRedundantSymbols)
                .NotEmpty().WithMessage(PhoneNumberIsEmpty)
                .MaximumLength(16).WithMessage(PhoneNumberTooLong);

            RuleFor(command => command.FirstName)
                .NotEmpty().WithMessage(FirstNameIsEmpty);

            RuleFor(command => command.LastName)
                .NotEmpty().WithMessage(LastNameIsEmpty);

            Transform(command => command.PassportNumber, RemoveAllRedundantSymbols)
                .NotEmpty().WithMessage(PassportNumberIsEmpty)
                .Matches(@"^\d{10}$").WithMessage(PassportNumberHasWrongFormat);                     
                       
            Transform(command => command.Inn, RemoveAllRedundantSymbols)
                .Matches(@"^$|^\d{12}$").WithMessage(InnHasWrongFormat);

            RuleFor(command => command.FormData)
                .NotEmpty().WithMessage(FormDataIsEmpty);
        }

        public async Task<bool> BeUniqueId(Guid id, CancellationToken cancellationToken)
            => !await _dbContext.Registrations.AnyAsync(r => r.Id == id, cancellationToken);
        

        private string RemoveAllRedundantSymbols(string? value)
            => value is null
                    ? string.Empty
                    : Regex.Replace(value, @"[+()\s\-]", string.Empty);
    }
}
