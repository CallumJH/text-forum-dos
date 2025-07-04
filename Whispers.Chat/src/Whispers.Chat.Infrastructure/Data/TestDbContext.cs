//using Microsoft.EntityFrameworkCore;
//using Whispers.Chat.Core.BoundedContexts.IdentityAndUsers.Aggregates;
//using Whispers.Chat.Core.BoundedContexts.Posts;
//using Whispers.Chat.Core.BoundedContexts.SiteModeration.Aggregates;
//using Whispers.Chat.Core.Generated.ContributorAggregate;
//using Whispers.Chat.Infrastructure.Data;
//using Whispers.Chat.Infrastructure.Persistence.Configurations;

//namespace Whispers.Chat.IntegrationTests.Data;

//// Create a concrete test DbContext
//public class TestDbContext : BaseAppDbContext<TestDbContext>
//{
//  public TestDbContext(DbContextOptions<TestDbContext> options, IDomainEventDispatcher? dispatcher)
//      : base(options, dispatcher) { }

//  //public TestDbContext(DbContextOptions<TestDbContext> options)
//  //    : base(options) { }

//  // Add all the DbSets you need for testing
//  public DbSet<Contributor> Contributors => Set<Contributor>();
//  public DbSet<User> Users => Set<User>();
//  public DbSet<Post> Posts => Set<Post>();
//  public DbSet<Moderator> Moderators => Set<Moderator>();

//  protected override void OnModelCreating(ModelBuilder modelBuilder)
//  {
//    base.OnModelCreating(modelBuilder);
//    // Apply all your entity configurations
//    modelBuilder.ApplyConfigurationsFromAssembly(typeof(ModeratorConfiguration).Assembly);
//    modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostConfiguration).Assembly);
//    modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
//  }
//}
