using MediatR;

namespace ddd.Domain
{
    // Marker interface — event domenowy
    public interface IDomainEvent : INotification { }

    // Klasa bazowa eventu (opcjonalna)
    public abstract class DomainEventBase : IDomainEvent
    {
        public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;
    }
}
