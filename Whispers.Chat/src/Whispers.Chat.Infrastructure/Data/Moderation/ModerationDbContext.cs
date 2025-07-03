using Whispers.Chat.Core.BoundedContexts.SiteModeration.Aggregates;

namespace Whispers.Chat.Infrastructure.Data.Moderation;

public class ModerationContext : AppDbContext
{
  public ModerationContext(
      DbContextOptions<AppDbContext> options,
      IDomainEventDispatcher domainEventDispatcher) : base(options, domainEventDispatcher)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Moderator>(b =>
    {
      b.ToTable("Moderators");
    });
  }
}
