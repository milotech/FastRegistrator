using FluentValidation;
using System.Text.RegularExpressions;

namespace FastRegistrator.ApplicationCore.Commands.CheckPersonByPhone
{
    public class CheckPersonByPhoneCommandValidator : AbstractValidator<CheckPersonByPhoneCommand>
    {
        public const string MobilePhoneIsEmpty = "Mobile phone is empty.";
        public const string MobilePhoneHasWrongFormat = "Mobile phone has wrong format.";

        public CheckPersonByPhoneCommandValidator() 
        {
            Transform(command => command.PhoneNumber, RemoveAllRedundantSymbols)
                .NotEmpty().WithMessage(MobilePhoneIsEmpty)
                .Matches(@"^(7|8)\d{10}$").WithMessage(MobilePhoneHasWrongFormat);
        }

        private string RemoveAllRedundantSymbols(string value)
            => Regex.Replace(value, @"[+()\s\-]", string.Empty);
    }
}
