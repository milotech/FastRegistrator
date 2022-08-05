using MediatR;
using System.Threading.Channels;

namespace FastRegistrator.Infrastructure.CommandExecutor
{
    internal class CommandsQueue
    {
        private readonly Channel<IBaseRequest> _queue;
        private readonly Func<IBaseRequest, CancellationToken, Task> _executeAction;
        private readonly CancellationToken _cancel;

        public CommandsQueue(int maxParallelExecutions, Func<IBaseRequest, CancellationToken, Task> executeAction, CancellationToken cancel)
        {
            if(maxParallelExecutions <= 0) 
                throw new ArgumentException(nameof(maxParallelExecutions));

            _queue = Channel.CreateUnbounded<IBaseRequest>(new UnboundedChannelOptions
            {
                SingleWriter = false,
                SingleReader = (maxParallelExecutions == 1)
            });
            _executeAction = executeAction;
            _cancel = cancel;

            StartQueueConsumers(maxParallelExecutions);
        }

        public void Enqueue(IBaseRequest request)
        {
            _queue.Writer.WriteAsync(request, _cancel);
        }

        private void StartQueueConsumers(int consumersCount)
        {
            for(int i = 0; i < consumersCount; i++)
            {
                Task.Run(ConsumerWork);
            }
        }

        private async Task ConsumerWork()
        {
            try
            {
                while (await _queue.Reader.WaitToReadAsync(_cancel))
                {
                    while (_queue.Reader.TryRead(out IBaseRequest? request))
                    {
                        await _executeAction(request, _cancel);
                    }
                }
            }
            catch (OperationCanceledException) { /* stop the consumer */ }
        }
    }
}
