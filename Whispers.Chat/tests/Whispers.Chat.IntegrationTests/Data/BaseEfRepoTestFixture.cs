using Whispers.Chat.Core.Generated.ContributorAggregate;
using Whispers.Chat.Infrastructure.Data;

namespace Whispers.Chat.IntegrationTests.Data;

public abstract class BaseEfRepoTestFixture
{
  protected AppDbContext _dbContext;

  protected BaseEfRepoTestFixture()
  {
    var options = CreateNewContextOptions();
    var fakeEventDispatcher = Substitute.For<IDomainEventDispatcher>();
    _dbContext = new AppDbContext(options, fakeEventDispatcher); // Remove the asterisks
  }

  protected static DbContextOptions<AppDbContext> CreateNewContextOptions()
  {
    var serviceProvider = new ServiceCollection()
        .AddEntityFrameworkInMemoryDatabase()
        .BuildServiceProvider();

    var builder = new DbContextOptionsBuilder<AppDbContext>();
    builder.UseInMemoryDatabase("cleanarchitecture")
           .UseInternalServiceProvider(serviceProvider);
    return builder.Options;
  }

  protected EfRepository<Contributor> GetRepository()
  {
    return new EfRepository<Contributor>(_dbContext);
  }
}
