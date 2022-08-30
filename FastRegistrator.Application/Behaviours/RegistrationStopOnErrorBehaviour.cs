using FastRegistrator.Application.Commands;
using FastRegistrator.Application.Commands.CompleteRegistration;
using FastRegistrator.Application.Exceptions;
using FastRegistrator.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FastRegistrator.Application.Behaviours;

public class RegistrationStopOnErrorBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>, IRegistrationStopOnErrorTrigger
{
    private readonly ICommandExecutor _cmdExecutor;
    private readonly ILogger<TRequest> _logger;

    public RegistrationStopOnErrorBehaviour(ICommandExecutor cmdExecutor, ILogger<TRequest> logger)
    {
        _cmdExecutor = cmdExecutor;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        try
        {
            return await next();
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (RetryRequiredException)
        {
            throw;
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Triggering CompleteRegistrationWithErrorCommand execution");

            var stopRegistrationCommand = new CompleteRegistrationWithErrorCommand(
                request.RegistrationId, ex.Message, Domain.Enums.ErrorSource.FastRegistrator
            );
            _ = _cmdExecutor.Execute(stopRegistrationCommand);

            throw;
        }
    }
}
