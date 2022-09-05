using FastRegistrator.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FastRegistrator.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        private ICommandExecutor _commandExecutor;
        private ISender _queryExecutor;

        public ApiControllerBase(ICommandExecutor commandExecutor, ISender queryExecutor)
        {
            _commandExecutor = commandExecutor;
            _queryExecutor = queryExecutor;
        }

        protected Task<TResponse> ExecuteCommand<TResponse>(IRequest<TResponse> command, CancellationToken? cancel = null)
        {
            return _commandExecutor.Execute(command, cancel);
        }

        protected Task ExecuteCommand(IRequest command, CancellationToken? cancel = null)
        {
            return _commandExecutor.Execute(command, cancel);
        }

        protected Task<TResponse> ExecuteQuery<TResponse>(IRequest<TResponse> query, CancellationToken cancel)
        {
            return _queryExecutor.Send(query, cancel);
        }
    }
}
