using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Whispers.Chat.SharedKernel;

namespace Whispers.Chat.Core.DomainEvents;

public class DomainEventDispatcher : IDomainEventDispatcher
{
  private readonly IMediator _mediator;

  public DomainEventDispatcher(IMediator mediator)
  {
    _mediator = mediator;
  }

  public async Task DispatchAndClearEvents(IEnumerable<BaseEntity> entitiesWithEvents)
  {
    foreach (var entity in entitiesWithEvents)
    {
      var events = entity.DomainEvents.ToArray();
      entity.ClearDomainEvents();
      foreach (var domainEvent in events)
      {
        await _mediator.Publish(domainEvent);
      }
    }
  }
} 