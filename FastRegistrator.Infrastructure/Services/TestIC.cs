using FastRegistrator.ApplicationCore.DTOs.ICRegistrationDTOs;
using FastRegistrator.ApplicationCore.Interfaces;
using System.Net;

namespace FastRegistrator.Infrastructure.Services
{
    public class TestIC : ITestIC
    {
        public TestIC()
        { }

        public async Task<HttpResponseMessage> SendDataAsync(ICRegistrationData registrationData, CancellationToken cancellationToken)
        {
            var random = new Random();
            var httpResponseMessage = new HttpResponseMessage();

            if (random.Next(0, 1) % 2 == 0)
            {
                httpResponseMessage.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                var error = "Not found error";
                var content = new StringContent(error);
                httpResponseMessage.Content = content;
                httpResponseMessage.StatusCode = HttpStatusCode.NotFound;
            }

            await Task.Delay(10000, cancellationToken);

            return httpResponseMessage;
        }
    }
}
