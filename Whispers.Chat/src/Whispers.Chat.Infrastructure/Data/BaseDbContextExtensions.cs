namespace Whispers.Chat.Infrastructure.Data;

public static class BaseDbContextExtensions
{
  public static void AddApplicationDbContext(this IServiceCollection services, string connectionString) =>
    services.AddDbContext<BaseDbContext>(options =>
         options.UseSqlite(connectionString));

}
