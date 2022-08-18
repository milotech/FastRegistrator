using FastRegistrator.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FastRegistrator.Infrastructure.EventBus
{
    public class RabbitMqConnection : IDisposable
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<RabbitMqConnection> _logger;

        private IConnection? _connection;
        private bool _disposed;
        private object _sync = new object();

        public RabbitMqConnection(IOptions<EventBusConnectionSettings> settings, ILogger<RabbitMqConnection> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _connectionFactory = CreateConnectionFactory(settings.Value);
        }

        public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;

        public IModel CreateChannel()
        {
            return !IsConnected
                ? throw new InvalidOperationException("No RabbitMQ connections are available to perform this action")
                : _connection!.CreateModel();
        }

        public void Connect()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("Connection disposed");
            }

            lock (_sync)
            {
                if (!IsConnected)
                {
                    _logger.LogInformation("RabbitMQ Client trying to connect...");

                    try
                    {
                        if (_connection != null)
                        {
                            _connection.Dispose();
                        }

                        _connection = _connectionFactory.CreateConnection();

                        _connection.ConnectionShutdown += OnConnectionShutdown;
                        _connection.CallbackException += OnCallbackException;
                        _connection.ConnectionBlocked += OnConnectionBlocked;

                        _logger.LogInformation("RabbitMQ Client acquired a connection to '{HostName}'", _connection.Endpoint.HostName);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to open RabbitMQ connection. ");
                        throw;
                    }
                }
            }
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            try
            {
                if (_connection != null)
                {
                    _logger.LogInformation("Close RabbitMQ connection");

                    _connection.ConnectionShutdown -= OnConnectionShutdown;
                    _connection.CallbackException -= OnCallbackException;
                    _connection.ConnectionBlocked -= OnConnectionBlocked;

                    _connection.Close();
                    _connection.Dispose();
                    _connection = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on closing the connection");
            }
        }

        private void OnConnectionBlocked(object? sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed)
            {
                return;
            }

            _logger.LogWarning("A RabbitMQ connection is shutdown. Trying to re-connect...");

            //CheckConnection();
        }

        private void OnCallbackException(object? sender, CallbackExceptionEventArgs e)
        {
            if (_disposed)
            {
                return;
            }

            _logger.LogWarning("A RabbitMQ connection throw exception. Trying to re-connect...");

            //CheckConnection();
        }

        private void OnConnectionShutdown(object? sender, ShutdownEventArgs reason)
        {
            if (_disposed)
            {
                return;
            }

            _logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");

            //CheckConnection();
        }

        private static IConnectionFactory CreateConnectionFactory(EventBusConnectionSettings settings)
        {
            var factory = new ConnectionFactory()
            {
                HostName = settings.Host,
                DispatchConsumersAsync = true,
            };

            if (settings.Ssl)
            {
                factory.Ssl = new SslOption() { Enabled = true, CertificateValidationCallback = (sender, cert, chain, errors) => true };
            }

            if (!string.IsNullOrEmpty(settings.VirtualHost))
            {
                factory.VirtualHost = settings.VirtualHost;
            }

            if (!string.IsNullOrEmpty(settings.User))
            {
                factory.UserName = settings.User;
            }

            if (!string.IsNullOrEmpty(settings.Password))
            {
                factory.Password = settings.Password;
            }

            return factory;
        }
    }
}
