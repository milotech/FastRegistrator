using FluentValidation;
using System.Text.RegularExpressions;

namespace FastRegistrator.ApplicationCore.Commands.SetStatusESIAApproved
{
    public class SetStatusESIAApprovedCommandValidator : AbstractValidator<SetStatusESIAApprovedCommand>
    {
        public const string MobilePhoneIsEmpty = "Mobile phone is empty.";
        public const string MobilePhoneHasWrongFormat = "Mobile phone has wrong format.";
        public const string FirstNameIsEmpty = "First name is empty.";
        public const string LastNameIsEmpty = "Last name is empty.";
        public const string SeriesIsEmpty = "Series is empty.";
        public const string SeriesHasWrongFormat = "Series has wrong format.";
        public const string NumberIsEmpty = "Number is empty.";
        public const string NumberHasWrongFormat = "Number has wrong format.";
        public const string IssuedByIsEmpty = "IssuedBy is empty.";
        public const string IssueIdIsEmpty = "Issue id is empty.";
        public const string IssueIdHasWrongFormat = "Issue id has wrong format.";
        public const string CitizenshipIsEmpty = "Citizenship is empty.";
        public const string SnilsIsEmpty = "Snils is empty.";
        public const string SnilsHasWrongFormat = "Snils has wrong format.";

        public SetStatusESIAApprovedCommandValidator()
        {
            Transform(command => command.PhoneNumber, RemoveAllRedundantSymbols)
                .NotEmpty().WithMessage(MobilePhoneIsEmpty)
                .Matches(@"^(7|8)\d{10}$").WithMessage(MobilePhoneHasWrongFormat);

            RuleFor(command => command.FirstName)
                .NotEmpty().WithMessage(FirstNameIsEmpty);

            RuleFor(command => command.LastName)
                .NotEmpty().WithMessage(LastNameIsEmpty);

            Transform(command => command.Series, RemoveAllRedundantSymbols)
                .NotEmpty().WithMessage(SeriesIsEmpty)
                .Matches(@"^\d{4}$").WithMessage(SeriesHasWrongFormat);

            Transform(command => command.Number, RemoveAllRedundantSymbols)
                .NotEmpty().WithMessage(NumberIsEmpty)
                .Matches(@"^\d{6}$").WithMessage(NumberHasWrongFormat);

            RuleFor(command => command.IssuedBy)
                .NotEmpty().WithMessage(IssuedByIsEmpty);

            Transform(command => command.IssueId, RemoveAllRedundantSymbols)
                .NotEmpty().WithMessage(IssueIdIsEmpty)
                .Matches(@"^\d{6}$").WithMessage(IssueIdHasWrongFormat);

            RuleFor(command => command.Citizenship)
                .NotEmpty().WithMessage(CitizenshipIsEmpty);

            Transform(command => command.Snils, RemoveAllRedundantSymbols)
                .NotEmpty().WithMessage(SnilsIsEmpty)
                .Matches(@"^\d{11}$").WithMessage(SnilsHasWrongFormat);
        }

        private string RemoveAllRedundantSymbols(string value)
            => Regex.Replace(value, @"[+()\s\-]", string.Empty);
    }
}
