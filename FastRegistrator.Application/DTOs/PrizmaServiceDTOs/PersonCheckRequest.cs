namespace FastRegistrator.ApplicationCore.DTOs.PrizmaServiceDTOs
{
    public record class PersonCheckRequest
    {
        public string Fio { get; init; } = null!;
        public string PassportNumber { get; init; } = null!;
        public string? Inn { get; init; }
        public DateTime? DateOfBirth { get; init; }
    }
}
