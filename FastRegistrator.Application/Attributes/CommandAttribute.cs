namespace FastRegistrator.Application.Attributes
{
    public enum CommandExecutionMode
    {
        // Execute command in the current thread and DI scope.
        InPlace,
        // Execute command in the new thread and new DI scope.
        Parallel,
        // Send command to queue with configured execution parallel degree.
        ExecutionQueue
    }

    public class CommandAttribute : Attribute
    {
        public CommandExecutionMode ExecutionMode { get; }
        public int ExecutionQueueParallelDegree = 1;

        public CommandAttribute(CommandExecutionMode executionMode)
        {
            ExecutionMode = executionMode;
        }
    }
}
