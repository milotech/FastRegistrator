using FastRegistrator.ApplicationCore.Attributes;
using FastRegistrator.ApplicationCore.Exceptions;
using FastRegistrator.ApplicationCore.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace FastRegistrator.Infrastructure.CommandExecutor
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly TimeSpan RetryDelay = TimeSpan.FromMinutes(1);

        private record class CommandExecutionOptions(CommandExecutionMode Mode, ICommandsQueue? Queue);

        private readonly Dictionary<Type, CommandExecutionOptions> _commandTypes;
        private readonly IMediator _mediator;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger _logger;
        private readonly CancellationToken _cancel;

        public CommandExecutor(
            IMediator mediator,
            IServiceScopeFactory scopeFactory,
            Assembly commandsAssembly,
            ILogger<CommandExecutor> logger,
            CancellationToken cancel)
        {
            _mediator = mediator;
            _scopeFactory = scopeFactory;
            _logger = logger;
            _cancel = cancel;

            _commandTypes = LoadCommandTypes(commandsAssembly);
        }

        public Task<TResponse> Execute<TResponse>(IRequest<TResponse> command, CancellationToken? cancel = null)
        {
            var type = command.GetType();
            if (!_commandTypes.ContainsKey(type))
                throw new InvalidOperationException("Unknown command type: " + type.Name);

            cancel ??= _cancel;

            switch(_commandTypes[type].Mode)
            {
                case CommandExecutionMode.InPlace:
                    return _mediator.Send(command, cancel.Value);
                case CommandExecutionMode.Parallel:
                    return ExecuteParallel(command, cancel.Value);
                case CommandExecutionMode.ExecutionQueue:                    
                    var queue = (_commandTypes[type].Queue as CommandsQueue<TResponse>)!;
                    var taskCompletion = new TaskCompletionSource<TResponse>();
                    var item = new CommandsQueueItem<TResponse>(command, taskCompletion);
                    queue.Enqueue(item);
                    return taskCompletion.Task;
                default:
                    throw new NotSupportedException(_commandTypes[type].Mode + " execution mode is not supported");
            };
        }

        public Task Execute(IRequest command, CancellationToken? cancel = null)
        {
            return Execute<Unit>(command, cancel);
        }

        private async Task<TResponse> ExecuteInScope<TResponse>(IRequest<TResponse> request, CancellationToken cancel)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    var response = await mediator.Send(request, cancel);
                    return response;
                }
            }
            catch(RetryRequiredException ex)
            {
                _logger.LogWarning($"Retry required for command {request.GetType().Name}: " + ex.Message);
                throw;
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation($"Executing command {request.GetType().Name} was cancelled");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled command exception");
                throw;
            }
        }

        private Task<TResponse> ExecuteParallel<TResponse>(IRequest<TResponse> request, CancellationToken cancel)
        {
            return Task.Run(async () =>
            {
                while (true)
                {
                    cancel.ThrowIfCancellationRequested();

                    try
                    {
                        return await ExecuteInScope(request, cancel);
                    }
                    catch (RetryRequiredException)
                    {
                        await Task.Delay(RetryDelay, cancel);
                    }                    
                }
            });            
        }

        private async Task ExecuteFromQueue<TResponse>(CommandsQueueItem<TResponse> queueItem, CancellationToken cancel)
        {
            try
            {
                var response = await ExecuteInScope(queueItem.Command, cancel);
                queueItem.TaskCompletion.SetResult(response);
            }
            catch(RetryRequiredException)
            {
                var type = queueItem.Command.GetType();
                var queue = (_commandTypes[type].Queue as CommandsQueue<TResponse>)!;
                _ = Task.Run(async () =>
                {
                    await Task.Delay(RetryDelay, _cancel);
                    queue.Enqueue(queueItem);
                });
            }
            catch(Exception ex)
            {
                queueItem.TaskCompletion.SetException(ex);
            }
        }

        private Dictionary<Type, CommandExecutionOptions> LoadCommandTypes(Assembly commandsAssembly)
        {
            var requestInterface = typeof(IRequest<>);

            return commandsAssembly.GetTypes()
                .Where(t => t.GetInterfaces().Contains(requestInterface))
                .Where(t => t.GetCustomAttributes<CommandAttribute>().Any())
                .ToDictionary(
                    t => t,
                    t =>
                    {
                        var attribute = t.GetCustomAttributes<CommandAttribute>().First();
                        ICommandsQueue? queue = null;
                        if (attribute.ExecutionMode == CommandExecutionMode.ExecutionQueue)
                        {
                            var responseType = t.GetGenericArguments()[0];
                            var queueType = typeof(CommandsQueue<>).MakeGenericType(responseType);
                            var queueItemType = typeof(CommandsQueueItem<>).MakeGenericType(responseType);
                            var funcType = typeof(Func<,>).MakeGenericType(queueItemType, typeof(CancellationToken), typeof(Task));
                            var func = Delegate.CreateDelegate(funcType, this.GetType().GetMethod("ExecuteFromQueue")!);
                            queue = Activator.CreateInstance(queueType, attribute.ExecutionQueueParallelDegree, func, _cancel) as ICommandsQueue;
                        }

                        return new CommandExecutionOptions
                        (
                            Mode: attribute.ExecutionMode,
                            Queue: queue
                        );
                    });
        }
    }
}
