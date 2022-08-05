using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastRegistrator.ApplicationCore.Interfaces
{
    public interface ICommandExecutor
    {
        Task<TResponse> Execute<TResponse>(IRequest<TResponse> command, CancellationToken? cancel = null);
        Task Execute(IRequest command, CancellationToken? cancel = null);
    }
}
