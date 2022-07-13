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
        
        private IConnection? _connection;
        private bool _disposed;
        private object _sync = new object();

        public RabbitMqConnection(IConnectionFactory connectionFactory, ILogger<RabbitMqConnection> logger)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

        public void Connect()
        {
            lock (_sync)
            {
                if (_disposed)
                    return;

                if (!IsConnected)
                {                    
                    _logger.LogInformation("RabbitMQ Client trying to connect...");

                    try
                    {
                        if (_connection != null)
                            _connection.Dispose();

                        _connection = _connectionFactory.CreateConnection();

                        _connection.ConnectionShutdown += OnConnectionShutdown;
                        _connection.CallbackException += OnCallbackException;
                        _connection.ConnectionBlocked += OnConnectionBlocked;

                        _logger.LogInformation("RabbitMQ Client acquired a connection to '{HostName}'", _connection.Endpoint.HostName);
                    }
                    catch(Exception ex)
                    {
                        _logger.LogError(ex, "Failed to open RabbitMQ connection. ");
                        throw;
                    }
                }
            }
        }

        public void Dispose()
        {
            if (_disposed) return;

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
            if (_disposed) return;

            _logger.LogWarning("A RabbitMQ connection is shutdown. Trying to re-connect...");

            //CheckConnection();
        }

        void OnCallbackException(object? sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("A RabbitMQ connection throw exception. Trying to re-connect...");

            //CheckConnection();
        }

        void OnConnectionShutdown(object? sender, ShutdownEventArgs reason)
        {
            if (_disposed) return;

            _logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");

            //CheckConnection();
        }
    }
}
