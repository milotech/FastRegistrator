﻿using FastRegistrator.ApplicationCore.IntegrationEvents.Events;
using FastRegistrator.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastRegistrator.ApplicationCore.IntegrationEvents.Handlers
{
    public class TestIntegrationEventHandler: IIntegrationEventHandler<TestIntegrationEvent>
    {
        private ILogger<TestIntegrationEventHandler> _logger;

        public TestIntegrationEventHandler(ILogger<TestIntegrationEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(TestIntegrationEvent @event)
        {
            _logger.LogInformation($"Test event! Value: {@event.Value}, Description: {@event.Description}");
            _logger.LogInformation("Started imitation of some work...");
            await Task.Delay(500);
            _logger.LogInformation("Completed.");
        }
    }
}