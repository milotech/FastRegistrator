using MediatR;
using System.Threading.Channels;

namespace FastRegistrator.Infrastructure.CommandExecutor
{
    internal interface ICommandsQueue
    {
    }

    internal class CommandsQueueItem<TResponse>
    {
        public IRequest<TResponse> Command { get; set; }
        public TaskCompletionSource<TResponse> TaskCompletion { get; set; }
        public CancellationToken Cancel { get; set; }

        public CommandsQueueItem(IRequest<TResponse> command, TaskCompletionSource<TResponse> taskCompletion, CancellationToken cancel)
        {
            TaskCompletion = taskCompletion;
            Command = command;
            Cancel = cancel;
        }
    }

    internal class CommandsQueue<TResponse> : ICommandsQueue
    {
        private readonly Channel<CommandsQueueItem<TResponse>> _queue;
        private readonly Func<CommandsQueueItem<TResponse>, Task> _executeAction;
        private readonly CancellationToken _cancel;

        public CommandsQueue(int maxParallelExecutions, Func<CommandsQueueItem<TResponse>, Task> executeAction, CancellationToken cancel)
        {
            if(maxParallelExecutions <= 0) 
                throw new ArgumentException(nameof(maxParallelExecutions));

            _queue = Channel.CreateUnbounded<CommandsQueueItem<TResponse>>(new UnboundedChannelOptions
            {
                SingleWriter = false,
                SingleReader = (maxParallelExecutions == 1)
            });
            _executeAction = executeAction;
            _cancel = cancel;

            StartQueueConsumers(maxParallelExecutions);
        }

        public void Enqueue(CommandsQueueItem<TResponse> queueItem)
        {
            _queue.Writer.WriteAsync(queueItem, _cancel);
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
                    while (_queue.Reader.TryRead(out CommandsQueueItem<TResponse>? item))
                    {
                        await _executeAction(item);
                    }
                }
            }
            catch (OperationCanceledException) { /* stop the consumer */ }
        }
    }
}
