﻿namespace FastRegistrator.ApplicationCore.Domain.Entities
{
    [Flags]
    public enum RejectionReason
    {
        None = 0,
        BlackListed = 1,
        BankruptcyRejected = 2,
        PassportRejected = 4
    }
}