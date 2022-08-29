using FastRegistrator.ApplicationCore.Commands.StartRegistration;
using System.Text;
using System.Text.Json;

namespace FastRegistrator.IntegrationTests
{
    public class RegistrationTests
        : IClassFixture<TestWebApplicationFactory>
    {
        private readonly TestWebApplicationFactory _factory;

        public RegistrationTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task QueryRegistrationStatus_NonExistentRegistration_ReturnsNotFound()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/registration/" + TestData.NonExistentRegistrationId);

            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("application/problem+json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Fact]
        public async Task QueryRegistrationStatus_ExistentRegistration_ReturnsOk()
        {

            var client = _factory.CreateClient();

            var response = await client.GetAsync("/registration/" + TestData.ExistentRegistrationId);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Fact]
        public async Task StartRegistration_NoMediaType_ReturnsUnsupportedMediaType()
        {
            var client = _factory.CreateClient();
            var content = new StringContent("");

            var response = await client.PostAsync("/registration/start", content);

            Assert.Equal(System.Net.HttpStatusCode.UnsupportedMediaType, response.StatusCode);
            Assert.Equal("application/problem+json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Fact]
        public async Task StartRegistration_InvalidData_ReturnsBadRequest()
        {
            var startRegistrationCommand = new StartRegistrationCommand()
            {
                RegistrationId = Guid.NewGuid(),
            };
            var client = _factory.CreateClient();
            var json = JsonSerializer.Serialize(startRegistrationCommand);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/registration/start", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/problem+json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
            Console.WriteLine(responseContent);
        }

    }
}
