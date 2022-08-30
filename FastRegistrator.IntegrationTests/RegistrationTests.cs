using FastRegistrator.ApplicationCore.Commands.CompleteRegistration;
using FastRegistrator.ApplicationCore.Commands.StartRegistration;
using FastRegistrator.ApplicationCore.Domain.Enums;
using FastRegistrator.ApplicationCore.DTOs.ICService;
using FastRegistrator.ApplicationCore.DTOs.PrizmaService;
using FastRegistrator.ApplicationCore.DTOs.RegistrationStatusQuery;
using FastRegistrator.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.TestHost;
using Moq;
using System.Net;
using System.Text;
using System.Text.Json;

namespace FastRegistrator.IntegrationTests
{
    public class RegistrationTests
        : IClassFixture<TestWebApplicationFactory>
    {
        private static class ContentTypes
        {
            public const string ProblemDetailsJson = "application/problem+json; charset=utf-8";
            public const string Json = "application/json; charset=utf-8";
        }

        private static class FastRegEndpoints
        {
            public const string RegistrationQuery = "/registration";
            public const string StartRegistration = "/registration/start";
            public const string CompleteRegistration = "/registration/complete";
        }

        private const string JsonMediaType = "application/json";

        private readonly TestWebApplicationFactory _factory;

        public RegistrationTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task QueryRegistrationStatus_NonExistentRegistration_ReturnsNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"{FastRegEndpoints.RegistrationQuery}/{TestData.NonExistentRegistrationId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(ContentTypes.ProblemDetailsJson, response.Content.Headers.ContentType?.ToString());
        }

        [Fact]
        public async Task QueryRegistrationStatus_ExistentRegistration_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"{FastRegEndpoints.RegistrationQuery}/{TestData.ExistentRegistrationId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(ContentTypes.Json, response.Content.Headers.ContentType?.ToString());
        }

        [Fact]
        public async Task StartRegistration_NoMediaType_ReturnsUnsupportedMediaType()
        {
            // Arrange
            var client = _factory.CreateClient();
            var content = new StringContent("");

            // Act
            var response = await client.PostAsync(FastRegEndpoints.StartRegistration, content);

            // Assert
            Assert.Equal(HttpStatusCode.UnsupportedMediaType, response.StatusCode);
            Assert.Equal(ContentTypes.ProblemDetailsJson, response.Content.Headers.ContentType?.ToString());
        }

        [Fact]
        public async Task StartRegistration_InvalidCommandData_ReturnsBadRequest()
        {
            // Arrange
            var startRegistrationCommand = new StartRegistrationCommand()
            {
                RegistrationId = Guid.NewGuid(),
            };
            var client = _factory.CreateClient();

            // Act
            var response = await PostRegistrationAsync(client, startRegistrationCommand);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(ContentTypes.ProblemDetailsJson, response.Content.Headers.ContentType?.ToString());
            Console.WriteLine(responseContent);
        }

        [Fact]
        public async Task StartRegistration_ClientIsBankrupt_CompleteWithPrizmaRejectedStatus()
        {
            // Arrange
            var prizmaServiceResponse = new PersonCheckResponse
            {
                HttpStatusCode = (int)HttpStatusCode.OK,
                PersonCheckResult = new PersonCheckResult
                {
                    PrizmaJsonResponse = "{}",
                    RejectionReason = RejectionReason.BankruptcyRejected
                }
            };
            var client = CreateClientWithServiceMocks(prizmaServiceResponse);
            var startRegistrationCommand = CreateValidStartRegistrationCommand();

            // Act
            var response = await PostRegistrationAsync(client, startRegistrationCommand);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var registrationStatusResponse = await WaitForRegistrationCompletionAsync(client, startRegistrationCommand.RegistrationId, 5000);            
            Assert.Equal(RegistrationStatus.PrizmaCheckRejected, registrationStatusResponse.Status);
            Assert.Equal(RejectionReason.BankruptcyRejected, registrationStatusResponse.PrizmaRejectionReason);
        }

        [Fact]
        public async Task StartRegistration_ICServiceRequestError_CompleteWithErrorStatus()
        {
            const string ICErrorMessage = "Unable to register client";

            // Arrange
            var icServiceResponse = new ICRegistrationResponse((int)HttpStatusCode.InternalServerError, new ICRegistrationError(ICErrorMessage, null));
            var client = CreateClientWithServiceMocks(icServiceResponse: icServiceResponse);
            var startRegistrationCommand = CreateValidStartRegistrationCommand();

            // Act
            var response = await PostRegistrationAsync(client, startRegistrationCommand);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var registrationStatusResponse = await WaitForRegistrationCompletionAsync(client, startRegistrationCommand.RegistrationId, 5000);
            Assert.Equal(RegistrationStatus.Error, registrationStatusResponse.Status);
            Assert.NotNull(registrationStatusResponse.Error);
            Assert.Equal(ErrorSource.IC, registrationStatusResponse.Error!.Source);
            Assert.Equal(ICErrorMessage, registrationStatusResponse.Error!.Message);
        }

        [Fact]
        public async Task StartRegistration_EverythingIsFine_CompleteWithAccountOpenedStatus()
        {
            // Arrange
            var startRegistrationCommand = CreateValidStartRegistrationCommand();
            var completeRegistrationCommand = new CompleteRegistrationByICCommand(startRegistrationCommand.PhoneNumber, ErrorMessage: null);
            var client = CreateClientWithServiceMocks(completeRegistrationCommand: completeRegistrationCommand);

            // Act
            var response = await PostRegistrationAsync(client, startRegistrationCommand);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var registrationStatusResponse = await WaitForRegistrationCompletionAsync(client, startRegistrationCommand.RegistrationId, 5000);
            Assert.Equal(RegistrationStatus.AccountOpened, registrationStatusResponse.Status);
            Assert.Null(registrationStatusResponse.Error);
        }


        private Task<HttpResponseMessage> PostRegistrationAsync(HttpClient client, StartRegistrationCommand command)
        {
            var json = JsonSerializer.Serialize(command);
            var content = new StringContent(json, Encoding.UTF8, JsonMediaType);
            return client.PostAsync("/registration/start", content);
        }

        private async Task<RegistrationStatusResponse> WaitForRegistrationCompletionAsync(HttpClient client, Guid registrationId, int waitTimeout)
        {
            var cancel = (new CancellationTokenSource(waitTimeout)).Token;

            while (!cancel.IsCancellationRequested)
            {
                var statusResponse = await client.GetAsync($"{FastRegEndpoints.RegistrationQuery}/{registrationId}");
                statusResponse.EnsureSuccessStatusCode();

                var registrationStatusResponse = await statusResponse.Content.ReadFromJsonAsync<RegistrationStatusResponse>();
                if (registrationStatusResponse!.Completed)
                    return registrationStatusResponse;

                await Task.Delay(500);
            }

            throw new TimeoutException();
        }

        private HttpClient CreateClientWithServiceMocks(
            PersonCheckResponse? prizmaServiceResponse = null,
            ICRegistrationResponse? icServiceResponse = null,
            CompleteRegistrationByICCommand? completeRegistrationCommand = null
            )
        {
            if (prizmaServiceResponse is null)
                prizmaServiceResponse = new PersonCheckResponse
                {
                    HttpStatusCode = (int)HttpStatusCode.OK,
                    PersonCheckResult = new PersonCheckResult { RejectionReason = RejectionReason.None, PrizmaJsonResponse = "{}" }
                };

            if (icServiceResponse is null)
                icServiceResponse = new ICRegistrationResponse { HttpStatusCode = (int)HttpStatusCode.OK };

            var prizmaService = new Mock<IPrizmaService>();
            prizmaService
                .Setup(p => p.PersonCheck(It.IsAny<PersonCheckRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(prizmaServiceResponse);

            var icService = new Mock<IICService>();
            var icServiceSendMethodSetup = icService
                .Setup(s => s.SendData(It.IsAny<ICRegistrationData>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(icServiceResponse);

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddTransient<IPrizmaService>((sp) => prizmaService.Object);
                    services.AddTransient<IICService>(sp => icService.Object);
                });
            })
            .CreateClient();

            if (completeRegistrationCommand is not null)
                icServiceSendMethodSetup.Callback(() =>
                {
                    var json = JsonSerializer.Serialize(completeRegistrationCommand);
                    var content = new StringContent(json, Encoding.UTF8, JsonMediaType);

                    client.PostAsync(FastRegEndpoints.CompleteRegistration, content);
                });

            return client;
        }
        
        private StartRegistrationCommand CreateValidStartRegistrationCommand()
        {
             return new StartRegistrationCommand()
             {
                 RegistrationId = Guid.NewGuid(),
                 FirstName = TestData.PersonName.FirstName,
                 MiddleName = TestData.PersonName.MiddleName,
                 LastName = TestData.PersonName.LastName,
                 PassportNumber = TestData.PassportNumber,
                 PhoneNumber = TestData.PhoneNumber,
                 FormData = "{}"
             };
        }
    }
}
