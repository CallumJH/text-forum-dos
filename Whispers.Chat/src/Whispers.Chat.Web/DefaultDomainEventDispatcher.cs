using Ardalis.SharedKernel;

public class DefaultDomainEventDispatcher : IDomainEventDispatcher
{
  public Task DispatchAndClearEvents(IEnumerable<EntityBase> entitiesWithEvents)
  {
    throw new NotImplementedException();
  }
}
