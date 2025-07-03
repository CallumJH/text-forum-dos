using Whispers.Chat.Core.Generated.ContributorAggregate;

namespace Whispers.Chat.Infrastructure.Data;

public static class SeedData
{
  public static readonly Contributor Contributor1 = new("Ardalis");
  public static readonly Contributor Contributor2 = new("Snowfrog");

  public static async Task InitializeAsync(BaseDbContext dbContext)
  {
    //await PopulateTestDataAsync(dbContext);
    dbContext.Database.EnsureCreated();
    await dbContext.SaveChangesAsync(); // TODO: Actually write seeding functionality here
  }

  public static async Task PopulateTestDataAsync(BaseDbContext dbContext)
  {
    await dbContext.SaveChangesAsync();
  }
}
