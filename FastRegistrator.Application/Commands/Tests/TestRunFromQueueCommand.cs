using FastRegistrator.ApplicationCore.Attributes;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FastRegistrator.ApplicationCore.Commands.Tests
{
    [Command(CommandExecutionMode.ExecutionQueue, ExecutionQueueParallelDegree = 4)]
    public record TestRunFromQueueCommand(string FIO) : IRequest;

    public class TestRunFromQueueCommandHandler : AsyncRequestHandler<TestRunFromQueueCommand>
    {
        private ILogger _logger;

        public TestRunFromQueueCommandHandler(ILogger<TestRunFromQueueCommand> logger)
        {
            _logger = logger;
        }

        protected override async Task Handle(TestRunFromQueueCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(request.FIO + ": Start work");

            await Task.Delay(1000);

            _logger.LogInformation(request.FIO + ": Work completed.");
        }
    }
}
