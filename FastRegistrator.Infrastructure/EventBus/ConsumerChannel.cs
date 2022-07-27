using FastRegistrator.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace FastRegistrator.Infrastructure.EventBus
{
    internal delegate Task NewEventHandler(IIntegrationEvent integrationEvent);

    internal class ConsumerChannel : IDisposable
    {
        private readonly RabbitMqConnection _connection;
        private readonly ILogger<ConsumerChannel> _logger;
        private readonly Type _eventType;
        private readonly string _exchangeName;
        private readonly string _routingKey;
        private readonly string _queueName;

        private IModel? _channel;
        private AsyncEventingBasicConsumer? _consumer;
        private bool _disposed;

        private string Name { get; }

        public bool IsAlive => !_disposed 
                            && _channel != null && _channel.IsOpen
                            && _consumer != null && _consumer.IsRunning;        

        public event NewEventHandler? OnNewEvent;

        public ConsumerChannel(RabbitMqConnection connection, ILogger<ConsumerChannel> logger,
            Type eventType, string exchangeName, string routingKey)
        {
            if (!eventType.GetInterfaces().Contains(typeof(IIntegrationEvent)))
            {
                throw new ArgumentException(
                    $"eventType:{eventType.Name} is not an implementation of IIntegrationEvent interface"
                );
            }

            _connection = connection;
            _logger = logger;
            _eventType = eventType;
            _exchangeName = exchangeName;
            _routingKey = routingKey;

            _queueName = eventType.Name;
            if (_queueName.EndsWith("Event"))
                _queueName += "s";

            Name = $"Consumer ({_queueName})";
        }

        public void Open()
        {
            _logger.LogInformation($"{Name}: Creating RabbitMQ channel");
            
            AsyncEventingBasicConsumer consumer;
            var channel = _connection.CreateChannel();

            channel.CallbackException += (sender, ea) =>
            {
                _logger.LogWarning(ea.Exception, $"{Name} channel: callback exception.");
            };

            try
            {
                channel.BasicQos(0, 1, false);
                channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Direct, durable: true);
                channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false);
                channel.QueueBind(queue: _queueName, exchange: _exchangeName, routingKey: _routingKey);

                consumer = new AsyncEventingBasicConsumer(channel);

                consumer.Received += Consumer_Received;
                
                consumer.Shutdown += (sender, ea) =>
                {
                    _logger.LogInformation($"{Name}: shutdown. " + ea.ToString());
                    return Task.CompletedTask;
                };

                consumer.ConsumerCancelled += (sender, ea) =>
                {
                    _logger.LogInformation($"{Name}: cancelled.");
                    return Task.CompletedTask;
                };

                consumer.Registered += (sender, ea) =>
                {
                    _logger.LogInformation($"{Name}: registered.");
                    return Task.CompletedTask;
                };

                channel.BasicConsume(
                    queue: _queueName,
                    autoAck: false,
                    consumer: consumer);
            }
            catch
            {
                channel.Dispose();
                throw;
            }

            _channel = channel;
            _consumer = consumer;
        }

        public void Close()
        {
            if (_channel != null)
            {
                try
                {
                    if (_consumer != null)
                    {
                        _consumer.Received -= Consumer_Received;
                        _consumer = null;
                    }
                    if (_channel.IsOpen)
                        _channel.Close();
                    _channel.Dispose();
                    _channel = null;                    
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{Name}: Error on closing channel");
                }
            }
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var routingKey = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

            try
            {
                if (OnNewEvent != null)
                {
                    var integrationEvent = JsonSerializer.Deserialize(message, _eventType) as IIntegrationEvent;
                    if (integrationEvent is null)
                    {
                        _logger.LogWarning($"{Name}: Received message that can't be deserialized to {_eventType.Name}. Message: {message}");
                    }
                    else
                    {
                        await OnNewEvent(integrationEvent);
                    }
                }
                else
                    _logger.LogWarning($"{Name}: OnNewEvent event handler is not set, message will be lost");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, $"Can't deserialize message to '{_eventType.Name}'. Message: {message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Name}: Unhandled exception while processing new message with routing key '{routingKey}'");
            }
            
            _channel!.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            Close();
        }
    }
}
