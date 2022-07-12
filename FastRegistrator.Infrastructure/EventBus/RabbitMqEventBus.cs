using FastRegistrator.ApplicationCore.Interfaces;
using FastRegistrator.Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastRegistrator.Infrastructure.EventBus
{
    public class RabbitMqEventBus : IEventBus, IDisposable
    {
        private record EventConfiguration(string ExchangeName, string RoutingKey);

        private readonly RabbitMqConnection _connection;
        private readonly ILogger<RabbitMqEventBus> _logger;
        private readonly IServiceProvider _serviceProvider;

        private Dictionary<Type, EventConfiguration> _eventConfigurations = new();
        private Dictionary<Type, Subscription> _subscriptions = new();        

        public RabbitMqEventBus(RabbitMqConnection connection, ILogger<RabbitMqEventBus> logger,
            IServiceProvider serviceProvider, int retryCount = 5)
        {
            _connection = connection;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public void ConfigureEvent<T>(string exchangeName, string routingKey)
            where T : IIntegrationEvent
        {
            Type eventType = typeof(T);

            if (_eventConfigurations.ContainsKey(eventType))
                throw new ArgumentException($"Event Type {eventType.Name} already configured");

            _eventConfigurations.Add(eventType, new EventConfiguration(exchangeName, routingKey));
        }

        public void Publish(IIntegrationEvent @event)
        {
            _connection.CheckConnection();
        }

        public void Subscribe<T, TH>(int consumersCount = 1)
            where T : IIntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            Type eventType = typeof(T);
            Type handlerType = typeof(TH);

            if(!_eventConfigurations.ContainsKey(eventType))
                throw new ArgumentException($"Event Type {eventType.Name} not configured");

            var eventConfig = _eventConfigurations[eventType];

            if (!_subscriptions.ContainsKey(eventType))
            {
                var queueName = eventType.Name;

                var consumerLogger = _serviceProvider.GetRequiredService<ILogger<ConsumerChannel>>();
                var consumerChannel = new ConsumerChannel(_connection, consumerLogger,
                    eventConfig.ExchangeName, eventConfig.RoutingKey, queueName);

                consumerChannel.Open();

                var subscription = new Subscription(consumerChannel, handlerType);
                _subscriptions.Add(eventType, subscription);
            }
            else
                _subscriptions[eventType].HandlerTypes.Add(handlerType);

            _logger.LogInformation($"Added {handlerType.Name} for {eventType.Name}");
        }

        public void Unsubscribe<T, TH>()
            where T : IIntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            Type eventType = typeof(T);
            Type handlerType = typeof(TH);

            if(_subscriptions.ContainsKey(eventType))
            {
                _subscriptions[eventType].HandlerTypes.Remove(handlerType);
                if (_subscriptions[eventType].HandlerTypes.Count == 0)
                {
                    _subscriptions[eventType].Consumer.Close();
                    _subscriptions.Remove(eventType);
                }
            }           
        }

        public void Dispose()
        {
            foreach (var subscription in _subscriptions.Values)
                subscription.Consumer.Close();

            _subscriptions.Clear();
        }

    }
}
