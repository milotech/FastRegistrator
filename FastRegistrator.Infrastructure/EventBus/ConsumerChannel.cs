using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace FastRegistrator.Infrastructure.EventBus
{
    internal delegate Task NewMessageHandler(string routingKey, string message);

    internal class ConsumerChannel : IDisposable
    {
        private readonly RabbitMqConnection _connection;
        private readonly ILogger<ConsumerChannel> _logger;
        private readonly string _exchangeName;
        private readonly string _routingKey;
        private readonly string _queueName;

        private IModel? _channel;
        private AsyncEventingBasicConsumer? _consumer;
        private bool _disposed;

        public bool IsAlive => !_disposed 
                            && _channel != null && _channel.IsOpen
                            && _consumer != null && _consumer.IsRunning;

        public event NewMessageHandler? OnNewMessage;

        public ConsumerChannel(RabbitMqConnection connection, ILogger<ConsumerChannel> logger,
            string exchangeName, string routingKey, string queueName)
        {
            _connection = connection;
            _logger = logger;
            _exchangeName = exchangeName;
            _routingKey = routingKey;
            _queueName = queueName;
        }

        public void Open()
        {
            _logger.LogInformation("Creating RabbitMQ consumer channel");
            
            AsyncEventingBasicConsumer consumer;
            var channel = _connection.CreateChannel();
            
            try
            {
                channel.BasicQos(0, 1, false);
                channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Direct, durable: true);
                channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false);
                channel.QueueBind(queue: _queueName, exchange: _exchangeName, routingKey: _routingKey);

                consumer = new AsyncEventingBasicConsumer(channel);
                consumer.Received += Consumer_Received;

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

            channel.CallbackException += (sender, ea) =>
            {
                _logger.LogWarning(ea.Exception, "Consumer channel callback exception.");
            };

            consumer.Shutdown += (sender, ea) =>
            {
                _logger.LogInformation("Consumer channel shutdown. " + ea.ToString());
                return Task.CompletedTask;
            };

            consumer.ConsumerCancelled += (sender, ea) =>
            {
                _logger.LogInformation($"Consumer for {_queueName} cancelled.");
                return Task.CompletedTask;
            };                

            _channel = channel;
            _consumer = consumer;
        }

        public void Close()
        {
            if(_channel != null)
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
                catch(Exception ex)
                {
                    _logger.LogError(ex, "Error on closing consumer channel");
                }
            }
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var routingKey = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

            try
            {
                if (OnNewMessage != null)
                {
                    await OnNewMessage(routingKey, message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unhandled exception while processing new message '{routingKey}'");
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
