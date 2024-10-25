using MediatR;

namespace Shared.DDD;

public interface IDomainEvent : INotification
{
    Guid EventId => Guid.NewGuid();
    public DateTime OccuredOn => DateTime.UtcNow;
    public string EventType => GetType().AssemblyQualifiedName!;
}
