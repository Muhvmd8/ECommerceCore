namespace ECommerce.Infrastructure.Caching
{
    public sealed class CacheEnvelope<T>
    {
        public required T Payload { get; init; }
        public DateTimeOffset CreatedAtUtc { get; init; }
        public DateTimeOffset LastAccessedAtUtc { get; init; }
    }
}
