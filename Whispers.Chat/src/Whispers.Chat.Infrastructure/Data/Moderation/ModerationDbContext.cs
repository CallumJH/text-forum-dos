using Whispers.Chat.Core.BoundedContexts.SiteModeration.Aggregates;

namespace Whispers.Chat.Infrastructure.Data.Moderation;

public class ModerationContext : BaseDbContext
{
  public ModerationContext(
      DbContextOptions<ModerationContext> options,
      IDomainEventDispatcher domainEventDispatcher) : base(options, domainEventDispatcher)
  {
  }

  public DbSet<Moderator> Moderators => Set<Moderator>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Moderator>(b =>
    {
      b.ToTable("Moderators");
    });
  }
}
