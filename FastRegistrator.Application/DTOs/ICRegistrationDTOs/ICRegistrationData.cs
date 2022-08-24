namespace FastRegistrator.ApplicationCore.DTOs.ICRegistrationDTOs
{
    public record class ICRegistrationData
    {
        public string Phone { get; init; } = null!;
        public string Data { get; init; } = null!;

        public ICRegistrationData(string phone, string data)
        {
            Phone = phone;
            Data = data;
        }
    }
}
