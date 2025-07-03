using System.Threading.Tasks;

namespace Whispers.Chat.Core.DomainEvents;

/// <summary>
/// Dispatches domain events to their handlers
/// </summary>
public interface IDomainEventDispatcher
{
  Task DispatchAndClearEvents(IEnumerable<BaseEntity> entitiesWithEvents);
} 