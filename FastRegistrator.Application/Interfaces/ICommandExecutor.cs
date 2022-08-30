using MediatR;

namespace FastRegistrator.Application.Interfaces
{
    public interface ICommandExecutor
    {
        Task<TResponse> Execute<TResponse>(IRequest<TResponse> command, CancellationToken? cancel = null);
        Task Execute(IRequest command, CancellationToken? cancel = null) 
            => Execute<Unit>(command, cancel);
    }
}
