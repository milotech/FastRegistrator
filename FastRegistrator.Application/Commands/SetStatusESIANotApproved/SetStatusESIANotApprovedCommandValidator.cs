using FastRegistrator.ApplicationCore.Commands.SetStatusESIANotApproved;
using FluentValidation;
using System.Text.RegularExpressions;

public class SetStatusESIANotApprovedCommandValidator : AbstractValidator<SetStatusESIANotApprovedCommand>
{
    public SetStatusESIANotApprovedCommandValidator()
    {
        Transform(x => x.PhoneNumber, RemoveAllRedundantSymbols)
            .NotEmpty().WithMessage("Mobile phone is empty.")
            .Matches(@"(7|8)\d{10}").WithMessage("Mobile phone has wrong format.");
    }

    private string RemoveAllRedundantSymbols(string value)
        => Regex.Replace(value, @"[+()\s\-]", string.Empty);
}