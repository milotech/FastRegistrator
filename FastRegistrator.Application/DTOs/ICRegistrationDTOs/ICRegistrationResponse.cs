namespace FastRegistrator.ApplicationCore.DTOs.ICRegistrationDTOs
{
    public class ICRegistrationResponse
    {
        public int StatusCode { get; set; }
        public ICRegistrationError? ICRegistrationError { get; set; }

        public ICRegistrationResponse()
        { }

        public ICRegistrationResponse(int statusCode, ICRegistrationError? iCRegistrationError)
        {
            StatusCode = statusCode;
            ICRegistrationError = iCRegistrationError;
        }
    }
}
