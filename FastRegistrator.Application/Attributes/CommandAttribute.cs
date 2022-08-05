namespace FastRegistrator.ApplicationCore.Attributes
{
    public enum CommandExecutionMode
    {
        // Execute command in the current thread and DI scope.
        InPlace,
        // Execute command in the new thread and new DI scope.
        // Command execution can be awaited and response can received.
        Parallel,
        // Send command to queue with configured execution parallel degree.
        // Response from command can't be used with this ExecutionMode.
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
