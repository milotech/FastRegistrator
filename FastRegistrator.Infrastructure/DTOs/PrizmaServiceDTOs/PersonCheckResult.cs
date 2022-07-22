namespace FastRegistrator.Infrastructure.DTOs.PrizmaServiceDTOs;

[Flags]
public enum RejectionReason
{
    None = 0,
    BlackListed = 1,
    BankruptcyRejected = 2,
    PassportRejected = 4
}

public class PersonCheckResult
{
    public bool Result
    {
        get => RejectionReason == RejectionReason.None;
    }
    public RejectionReason RejectionReason { get; set; }
    public string Response { get; set; } = null!;
}
