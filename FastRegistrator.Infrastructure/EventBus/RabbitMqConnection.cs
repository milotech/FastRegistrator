using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FastRegistrator.Infrastructure.EventBus
{
    public class RabbitMqConnection : IDisposable
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<RabbitMqConnection> _logger;
        private readonly int _retryCount;
        
        private IConnection? _connection;
        private bool _disposed;

        object _sync = new object();

        public RabbitMqConnection(IConnectionFactory connectionFactory, ILogger<RabbitMqConnection> logger, int retryCount = 5)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _retryCount = retryCount;
        }

        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen && !_disposed;
            }
        }

        public IModel CreateChannel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            return _connection!.CreateModel();
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;

            try
            {
                if (_connection != null)
                {
                    _connection.ConnectionShutdown -= OnConnectionShutdown;
                    _connection.CallbackException -= OnCallbackException;
                    _connection.ConnectionBlocked -= OnConnectionBlocked;
                    _connection.Dispose();
                }
            }
            catch (IOException ex)
            {
                _logger.LogCritical(ex.ToString());
            }
        }

        public bool CheckConnection()
        {
            _logger.LogInformation("RabbitMQ Client checking connection...");

            if (!IsConnected)
            {
                lock (_sync)
                {
                    if (!IsConnected)
                    {
                        _logger.LogInformation("RabbitMQ Client trying to connect...");

                        var policy = RetryPolicy
                            .Handle<SocketException>()
                            .Or<BrokerUnreachableException>()
                            .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                            {
                                _logger.LogWarning(ex, "RabbitMQ Client could not connect after {TimeOut}s ({ExceptionMessage})", $"{time.TotalSeconds:n1}", ex.Message);
                            }
                        );

                        policy.Execute(() =>
                        {
                            _connection = _connectionFactory.CreateConnection();
                        });

                        if (IsConnected)
                        {
                            _connection!.ConnectionShutdown += OnConnectionShutdown;
                            _connection.CallbackException += OnCallbackException;
                            _connection.ConnectionBlocked += OnConnectionBlocked;

                            _logger.LogInformation("RabbitMQ Client acquired a persistent connection to '{HostName}' and is subscribed to failure events", _connection.Endpoint.HostName);

                            return true;
                        }
                        else
                        {
                            _logger.LogCritical("FATAL ERROR: RabbitMQ connections could not be created and opened");

                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private void OnConnectionBlocked(object? sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("A RabbitMQ connection is shutdown. Trying to re-connect...");

            CheckConnection();
        }

        void OnCallbackException(object? sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("A RabbitMQ connection throw exception. Trying to re-connect...");

            CheckConnection();
        }

        void OnConnectionShutdown(object? sender, ShutdownEventArgs reason)
        {
            if (_disposed) return;

            _logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");

            CheckConnection();
        }
    }
}
