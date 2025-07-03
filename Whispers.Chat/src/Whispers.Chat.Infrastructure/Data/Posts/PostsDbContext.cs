using Whispers.Chat.Core.BoundedContexts.Posts;

namespace Whispers.Chat.Infrastructure.Data.Posts;

public class PostsContext : AppDbContext
{
  public PostsContext(
      DbContextOptions<AppDbContext> options,
      IDomainEventDispatcher domainEventDispatcher) : base(options, domainEventDispatcher)
  {
  }
    public DbSet<Comment> Comments => Set<Comment>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Post>(b =>
    {
      b.ToTable("Posts");
      b.HasMany(p => p.Comments)
           .WithOne()
           .HasForeignKey(c => c.PostId)
           .OnDelete(DeleteBehavior.Cascade);
    });

    modelBuilder.Entity<Comment>(b =>
    {
      b.ToTable("Comments");
    });
  }
}
