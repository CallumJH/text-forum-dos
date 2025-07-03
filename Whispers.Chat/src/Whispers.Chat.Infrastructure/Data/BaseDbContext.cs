using Ardalis.Specification;

namespace Whispers.Chat.Infrastructure.Data;

public class BaseDbContext : DbContext
{
  private readonly IDomainEventDispatcher _domainEventDispatcher;

  public BaseDbContext(DbContextOptions options, IDomainEventDispatcher domainEventDispatcher) : base(options)
  {
    _domainEventDispatcher = domainEventDispatcher;
  }

  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
  {
    // Dispatch domain events before saving
    var domainEventEntities = ChangeTracker.Entries<EntityBase>()
        .Select(po => po.Entity)
        .Where(po => po.DomainEvents.Any())
        .ToArray();

    await _domainEventDispatcher.DispatchAndClearEvents(domainEventEntities);

    return await base.SaveChangesAsync(cancellationToken);
  }
}
