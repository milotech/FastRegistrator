using System.Net;

namespace FastRegistrator.ApplicationCore.DTOs.ICRegistrationDTOs
{
    public class ICRegistrationResponse
    {
        public int StatusCode { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
