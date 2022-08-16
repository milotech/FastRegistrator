using FastRegistrator.ApplicationCore.Domain.Enums;

namespace FastRegistrator.ApplicationCore.DTOs.PrizmaServiceDTOs;

public record class PersonCheckResult
{
    public bool Result
    {
        get => RejectionReason == RejectionReason.None;
    }
    public RejectionReason RejectionReason { get; init; }
    public string PrizmaJsonResponse { get; init; } = null!;
}
