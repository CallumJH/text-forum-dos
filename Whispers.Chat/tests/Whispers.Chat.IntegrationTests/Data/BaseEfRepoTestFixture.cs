using Whispers.Chat.Core.Generated.ContributorAggregate;
using Whispers.Chat.Infrastructure.Data;

namespace Whispers.Chat.IntegrationTests.Data;

public abstract class BaseEfRepoTestFixture
{
  protected BaseDbContext _dbContext;

  protected BaseEfRepoTestFixture()
  {
    // This test fixture is getting changed for now im keeping it out


    var options = CreateNewContextOptions();
    var _fakeEventDispatcher = Substitute.For<IDomainEventDispatcher>();

    _dbContext = new BaseDbContext(options, _fakeEventDispatcher);
  }

  protected static DbContextOptions<BaseDbContext> CreateNewContextOptions()
  {
    // Create a fresh service provider, and therefore a fresh
    // InMemory database instance.
    var serviceProvider = new ServiceCollection()
        .AddEntityFrameworkInMemoryDatabase()
        .BuildServiceProvider();

    // Create a new options instance telling the context to use an
    // InMemory database and the new service provider.
    var builder = new DbContextOptionsBuilder<BaseDbContext>();
    builder.UseInMemoryDatabase("cleanarchitecture")
           .UseInternalServiceProvider(serviceProvider);

    return builder.Options;
  }

  protected EfRepository<Contributor> GetRepository()
  {
    return new EfRepository<Contributor>(_dbContext);
  }
}
