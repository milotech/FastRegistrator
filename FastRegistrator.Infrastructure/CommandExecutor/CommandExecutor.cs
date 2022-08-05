using FastRegistrator.ApplicationCore.Attributes;
using FastRegistrator.ApplicationCore.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace FastRegistrator.Infrastructure.CommandExecutor
{
    public class CommandExecutor : ICommandExecutor
    {
        private record class CommandExecutionOptions(CommandExecutionMode Mode, CommandsQueue? Queue);

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
                    return Task.Run(() => ExecuteCommand(command, cancel.Value));
                case CommandExecutionMode.ExecutionQueue:
                    _commandTypes[type].Queue!.Enqueue(command);
                    return Task.FromResult(default(TResponse)!);
                default:
                    throw new NotSupportedException(_commandTypes[type].Mode + " execution mode is not supported");
            };
        }

        public Task Execute(IRequest command, CancellationToken? cancel = null)
        {
            return Execute<Unit>(command, cancel);
        }

        private async Task<TResponse> ExecuteCommand<TResponse>(IRequest<TResponse> request, CancellationToken cancel)
        {
            try
            {
                using(var scope = _scopeFactory.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    return await mediator.Send(request, cancel);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Unhandled command exception");
                throw;
            }
        }

        private async Task ExecuteCommand(IBaseRequest request, CancellationToken cancel)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    await mediator.Send(request, cancel);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled command exception");
            }
        }

        private Dictionary<Type, CommandExecutionOptions> LoadCommandTypes(Assembly commandsAssembly)
        {
            var requestInterface = typeof(IBaseRequest);

            return commandsAssembly.GetTypes()
                .Where(t => t.GetInterfaces().Contains(requestInterface))
                .Where(t => t.GetCustomAttributes<CommandAttribute>().Any())
                .ToDictionary(
                    t => t,
                    t =>
                    {
                        var attribute = t.GetCustomAttributes<CommandAttribute>().First();
                        return new CommandExecutionOptions
                        (
                            Mode: attribute.ExecutionMode,
                            Queue: attribute.ExecutionMode == CommandExecutionMode.ExecutionQueue
                                ? new CommandsQueue(attribute.ExecutionQueueParallelDegree, ExecuteCommand, _cancel)
                                : null
                        );
                    });
        }
    }
}
