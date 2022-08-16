using FastRegistrator.ApplicationCore.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FastRegistrator.ApplicationCore.Behaviours
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull, IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public LoggingBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation($"Running command: {request}");

            try
            {
                var result = await next();
                _logger.LogInformation($"Command completed : {request}");
                return result;
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch(ValidationException ex)
            {
                _logger.LogError(ex, $"Validation failed for the command {request}");
                throw;
            }
            catch (RetryRequiredException ex)
            {
                _logger.LogWarning($"Retry required for command {request}: " + ex.Message);
                throw;
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation($"Executing command {request} was cancelled");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unhandled exception for command {request}");

                throw;
            }
        }
    }
}
