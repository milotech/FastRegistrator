using FastRegistrator.Application.IntegrationEvents.Events;
using FastRegistrator.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace FastRegistrator.Application.IntegrationEvents.Handlers
{
    public class TestIntegrationEventHandler : IIntegrationEventHandler<TestIntegrationEvent>
    {
        private readonly ILogger<TestIntegrationEventHandler> _logger;

        public TestIntegrationEventHandler(ILogger<TestIntegrationEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(TestIntegrationEvent @event, CancellationToken cancel)
        {
            _logger.LogInformation($"Test event! Value: {@event.Value}, Description: {@event.Description}");
            _logger.LogInformation("Started imitation of some work...");
            await Task.Delay(500);
            _logger.LogInformation("Completed.");
        }
    }
}
