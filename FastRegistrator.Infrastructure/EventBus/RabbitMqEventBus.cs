using FastRegistrator.ApplicationCore.Exceptions;
using FastRegistrator.ApplicationCore.Interfaces;
using FastRegistrator.Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FastRegistrator.Infrastructure.EventBus
{
    // TODO: background worker for reopenning connection and consumers
    // TODO: publisher channels pool or single thread-safe publisher
    //       or pool of messages to publish with single sequential publisher
    // TODO: publisher confirms

    public class RabbitMqEventBus : IEventBus, IDisposable
    {
        private record EventConfiguration(string ExchangeName, string RoutingKey);

        private readonly RabbitMqConnection _connection;
        private readonly ILogger<RabbitMqEventBus> _logger;
        private readonly IServiceProvider _serviceProvider;

        private Dictionary<Type, EventConfiguration> _eventConfigurations = new();
        private Dictionary<Type, Subscription> _subscriptions = new();

        public RabbitMqEventBus(RabbitMqConnection connection, ILogger<RabbitMqEventBus> logger,
            IServiceProvider serviceProvider)
        {
            _connection = connection;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Binds IntegrationEvent type with RabbitMq Exchange name and optonally with routing key.
        /// If routing key is not specified, it will be taken from IntegrationEvent type name.
        /// </summary>        
        public void ConfigureEvent<T>(string exchangeName, string? routingKey = null)
            where T : IIntegrationEvent
        {
            Type eventType = typeof(T);

            if (_eventConfigurations.ContainsKey(eventType))
                throw new ArgumentException($"Event Type {eventType.Name} already configured");

            if (routingKey is null)
                routingKey = eventType.Name;

            _eventConfigurations.Add(eventType, new EventConfiguration(exchangeName, routingKey));
        }

        public void Publish(IIntegrationEvent integrationEvent)
        {
            Type eventType = integrationEvent.GetType();

            if (!_eventConfigurations.ContainsKey(eventType))
                throw new ArgumentException($"Event Type {eventType.Name} not configured");

            var eventConfig = _eventConfigurations[eventType];

            if (!_connection.IsConnected)
                _connection.Connect();

            var message = JsonSerializer.SerializeToUtf8Bytes(integrationEvent);

            using var publisher = new PublisherChannel(_connection,
                _serviceProvider.GetRequiredService<ILogger<PublisherChannel>>()
            );

            publisher.Open();
            publisher.Publish(eventConfig.ExchangeName, eventConfig.RoutingKey, message);
        }

        public void Subscribe<T, TH>()
            where T : IIntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            Type eventType = typeof(T);
            Type handlerType = typeof(TH);

            if(!_eventConfigurations.ContainsKey(eventType))
                throw new ArgumentException($"Event Type {eventType.Name} not configured");

            if (!_connection.IsConnected)
                _connection.Connect();

            var eventConfig = _eventConfigurations[eventType];

            if (!_subscriptions.ContainsKey(eventType))
            {
                var consumerLogger = _serviceProvider.GetRequiredService<ILogger<ConsumerChannel>>();
                var consumerChannel = new ConsumerChannel(_connection, consumerLogger,
                    eventType, eventConfig.ExchangeName, eventConfig.RoutingKey);

                consumerChannel.OnNewEvent += OnNewMessage;

                consumerChannel.Open();                

                var subscription = new Subscription(consumerChannel, handlerType);
                _subscriptions.Add(eventType, subscription);
            }
            else
                _subscriptions[eventType].HandlerTypes.Add(handlerType);

            _logger.LogInformation($"Added {handlerType.Name} for {eventType.Name}");
        }

        private async Task OnNewMessage(IIntegrationEvent integrationEvent)
        {
            Type eventType = integrationEvent.GetType();

            if(_subscriptions.TryGetValue(eventType, out Subscription? subscription))
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    foreach (Type handlerType in subscription.HandlerTypes)
                    {
                        var handler = ActivatorUtilities.CreateInstance(scope.ServiceProvider, handlerType);
                        await Task.Yield();
                        var handleMethod = handlerType.GetMethod("Handle");
                        await (Task)handleMethod!.Invoke(handler, new object[] { integrationEvent })!;
                    }
                }
            }
            else
            {
                _logger.LogWarning($"No subscription for '{eventType.Name}'");
            }
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
                    _subscriptions[eventType].Consumer.Dispose();
                    _subscriptions.Remove(eventType);
                }
            }           
        }

        public void Dispose()
        {
            foreach (var subscription in _subscriptions.Values)
                subscription.Consumer.Dispose();

            _connection.Dispose();
            _subscriptions.Clear();
        }
    }
}
