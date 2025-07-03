using Ardalis.SharedKernel;
using Whispers.Chat.Core.BoundedContexts.IdentityAndUsers.Aggregates;
using Whispers.Chat.Core.BoundedContexts.Posts;
using Whispers.Chat.Core.BoundedContexts.SiteModeration.Aggregates;
using Whispers.Chat.Core.Generated.ContributorAggregate;

namespace Whispers.Chat.Infrastructure.Data;

public class AppDbContext : DbContext
{
  private readonly IDomainEventDispatcher? _dispatcher;

  public AppDbContext(DbContextOptions<AppDbContext> options, IDomainEventDispatcher? dispatcher = null)
      : base(options)
  {
    _dispatcher = dispatcher;
  }

  // All your DbSets
  public DbSet<Contributor> Contributors => Set<Contributor>();
  public DbSet<User> Users => Set<User>();
  public DbSet<Post> Posts => Set<Post>();
  public DbSet<Moderator> Moderators => Set<Moderator>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }

  // Handle domain events if you need them
  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
  {
    // Dispatch domain events before saving
    var domainEventEntities = ChangeTracker.Entries<EntityBase>()
        .Select(po => po.Entity)
        .Where(po => po.DomainEvents.Any())
        .ToArray();
    if(domainEventEntities.Any() && _dispatcher is not null)
    {
      await _dispatcher.DispatchAndClearEvents(domainEventEntities);
    }

    return await base.SaveChangesAsync(cancellationToken);
  }
}
