namespace FastRegistrator.ApplicationCore.DTOs.PrizmaServiceDTOs;

[Flags]
public enum RejectionReason
{
    None = 0,
    BlackListed = 1,
    BankruptcyRejected = 2,
    PassportRejected = 4
}

public record class PersonCheckResult
{
    public bool Result
    {
        get => RejectionReason == RejectionReason.None;
    }
    public RejectionReason RejectionReason { get; init; }
    public string PrizmaJsonResponse { get; init; } = null!;
}
