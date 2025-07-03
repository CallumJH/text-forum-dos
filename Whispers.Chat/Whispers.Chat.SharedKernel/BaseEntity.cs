using MediatR;

namespace Whispers.Chat.SharedKernel;

/// <summary>
/// Base class for domain events
/// </summary>
public abstract record BaseDomainEvent(DateTimeOffset OccurredOn) : INotification
{
  protected BaseDomainEvent() : this(DateTimeOffset.UtcNow) { }
}

/// <summary>
/// Interface for domain event handlers
/// </summary>
public interface IDomainEventHandler<in T> : INotificationHandler<T> where T : BaseDomainEvent
{
}

/// <summary>
/// Interface for entities that need audit information
/// </summary>
public interface IAuditableEntity
{
  DateTime DateCreated { get; }
  DateTime? DateUpdated { get; }
  Guid CreatedBy { get; }
  Guid? UpdatedBy { get; }
}
