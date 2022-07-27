using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace FastRegistrator.Infrastructure.EventBus
{
    internal class PublisherChannel : IDisposable
    {
        private readonly RabbitMqConnection _connection;
        private readonly ILogger<PublisherChannel> _logger;

        private IModel? _channel;
        private bool _disposed;

        public bool IsAlive => !_disposed && _channel != null && _channel.IsOpen;

        public PublisherChannel(RabbitMqConnection connection, ILogger<PublisherChannel> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public void Open()
        {
            _logger.LogInformation("Creating RabbitMQ publisher channel");

            _channel = _connection.CreateChannel();

            _channel.CallbackException += (sender, ea) =>
            {
                _logger.LogWarning(ea.Exception, "Publisher channel callback exception.");
            };

            _channel.ModelShutdown += (sender, ea) =>
            {
                _logger.LogInformation("Publisher channel shutdown. " + ea.ToString());
            };

            _channel.BasicReturn += (sender, ea) =>
            {
                _logger.LogWarning($"Message returns. Exchange: {ea.Exchange}. Reply: ({ea.ReplyCode}) {ea.ReplyText}");
            };
        }

        public void Publish(string exchangeName, string routingKey, byte[] message)
        {
            if (_channel == null)
                throw new InvalidOperationException("Publisher Channel is not open");

            var properties = _channel.CreateBasicProperties();
            properties.DeliveryMode = 2;

            _channel.BasicPublish(
                exchange: exchangeName,
                routingKey: routingKey,
                mandatory: true,
                basicProperties: properties,
                body: message
                );
        }

        public void Close()
        {
            if (_channel != null)
            {
                try
                {                    
                    if (_channel.IsOpen)
                        _channel.Close();
                    _channel.Dispose();
                    _channel = null;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error on closing consumer channel");
                }
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            Close();
        }
    }
}
