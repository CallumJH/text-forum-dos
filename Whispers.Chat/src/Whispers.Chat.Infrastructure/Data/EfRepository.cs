namespace Whispers.Chat.Infrastructure.Data;

// inherit from Ardalis.Specification type
public class EfRepository<T>(BaseDbContext dbContext) :
  RepositoryBase<T>(dbContext), IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
{
}
