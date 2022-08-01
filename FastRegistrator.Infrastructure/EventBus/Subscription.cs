namespace FastRegistrator.Infrastructure.EventBus
{
    internal class Subscription
    {
        public HashSet<Type> HandlerTypes { get; } = new HashSet<Type>();
        public ConsumerChannel Consumer { get; }

        public Subscription(ConsumerChannel consumer, params Type[] handlerTypes)
        {
            Consumer = consumer;
            foreach (var handlerType in handlerTypes)
                HandlerTypes.Add(handlerType);
        }
    }
}
