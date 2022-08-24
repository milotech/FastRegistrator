namespace FastRegistrator.ApplicationCore.DTOs.ICRegistrationDTOs
{
    public class ICRegistrationResponse
    {
        public int HttpStatusCode { get; set; }
        public ICRegistrationError? ICRegistrationError { get; set; }

        public ICRegistrationResponse()
        { }

        public ICRegistrationResponse(int httpStatusCode, ICRegistrationError? iCRegistrationError)
        {
            HttpStatusCode = httpStatusCode;
            ICRegistrationError = iCRegistrationError;
        }
    }
}
