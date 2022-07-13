using FluentValidation;
using System.Text.RegularExpressions;

namespace FastRegistrator.ApplicationCore.Commands.CheckUserByMobilePhone
{
    public class CheckUserByMobilePhoneCommandValidator : AbstractValidator<CheckUserByMobilePhoneCommand>
    {
        public CheckUserByMobilePhoneCommandValidator() 
        {
            Transform(x => x.MobilePhone, RemoveAllRedundantSymbols)
                .NotEmpty().WithMessage("Mobile phone is empty.")
                .Matches(@"(7|8)\d{10}").WithMessage("Mobile phone has wrong format.");
        }

        private string RemoveAllRedundantSymbols(string value)
            => Regex.Replace(value, @"[+()\s\-]", string.Empty);
    }
}
