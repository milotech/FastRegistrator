﻿namespace FastRegistrator.ApplicationCore.DTOs.PrizmaServiceDTOs
{
    public class PersonCheckRequest
    {
        public string Fio { get; init; } = null!;
        public string PassportNumber { get; init; } = null!;
        public string? IndividualFormId { get; init; }
        public string? Inn { get; init; }
        public string? DateOfBirth { get; init; }
    }
}