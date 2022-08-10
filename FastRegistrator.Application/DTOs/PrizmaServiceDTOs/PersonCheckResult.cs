using FastRegistrator.ApplicationCore.Domain.Enums;

namespace FastRegistrator.ApplicationCore.DTOs.PrizmaServiceDTOs;

public class PersonCheckResult
{
    public bool Result
    {
        get => RejectionReason == RejectionReason.None;
    }
    public RejectionReason RejectionReason { get; set; }
    public string PrizmaJsonResponse { get; set; } = null!;
}
