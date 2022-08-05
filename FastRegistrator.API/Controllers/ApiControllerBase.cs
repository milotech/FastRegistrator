using FastRegistrator.ApplicationCore.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FastRegistrator.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        private ICommandExecutor? _commandExecutor;
        private ISender? _queryExecutor;

        private ICommandExecutor CommandExecutor => _commandExecutor ??= HttpContext.RequestServices.GetRequiredService<ICommandExecutor>();
        private ISender QueryExecutor => _queryExecutor ??= HttpContext.RequestServices.GetRequiredService<ISender>();

        protected Task<TResponse> ExecuteCommand<TResponse>(IRequest<TResponse> command, CancellationToken cancel)
        {
            return CommandExecutor.Execute(command, cancel);
        }

        protected Task ExecuteCommand(IRequest command, CancellationToken cancel)
        {
            return CommandExecutor.Execute(command, cancel);
        }

        protected Task<TResponse> ExecuteQuery<TResponse>(IRequest<TResponse> query, CancellationToken cancel)
        {
            return QueryExecutor.Send(query, cancel);
        }
    }
}
