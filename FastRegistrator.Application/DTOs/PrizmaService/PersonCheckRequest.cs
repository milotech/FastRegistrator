namespace FastRegistrator.ApplicationCore.DTOs.PrizmaService
{
    public record class PersonCheckRequest
    {
        public string Fio { get; init; } = null!;
        public string PassportNumber { get; init; } = null!;
        public string? Inn { get; init; }
        public DateTime? DateOfBirth { get; init; }
    }
}
