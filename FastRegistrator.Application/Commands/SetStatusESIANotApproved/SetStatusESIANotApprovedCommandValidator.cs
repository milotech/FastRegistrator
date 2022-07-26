using FastRegistrator.ApplicationCore.Commands.SetStatusESIANotApproved;
using FluentValidation;
using System.Text.RegularExpressions;

public class SetStatusESIANotApprovedCommandValidator : AbstractValidator<SetStatusESIANotApprovedCommand>
{
    public const string MobilePhoneIsEmpty = "Mobile phone is empty.";
    public const string MobilePhoneHasWrongFormat = "Mobile phone has wrong format.";

    public SetStatusESIANotApprovedCommandValidator()
    {
        Transform(command => command.PhoneNumber, RemoveAllRedundantSymbols)
            .NotEmpty().WithMessage(MobilePhoneIsEmpty)
            .Matches(@"^(7|8)\d{10}$").WithMessage(MobilePhoneHasWrongFormat);
    }

    private string RemoveAllRedundantSymbols(string value)
        => Regex.Replace(value, @"[+()\s\-]", string.Empty);
}