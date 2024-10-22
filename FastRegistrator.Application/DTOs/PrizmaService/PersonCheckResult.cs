﻿using FastRegistrator.Application.Domain.Enums;

namespace FastRegistrator.Application.DTOs.PrizmaService;

public record class PersonCheckResult
{
    public bool Result => RejectionReason == RejectionReason.None;
    public RejectionReason RejectionReason { get; init; }
    public string PrizmaJsonResponse { get; init; } = null!;
}
