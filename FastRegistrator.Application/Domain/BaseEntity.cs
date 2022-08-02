namespace FastRegistrator.ApplicationCore.Domain
{
    public abstract class BaseEntity<T>
    {
        public T Id { get; protected set; } = default(T)!;
    }
}
