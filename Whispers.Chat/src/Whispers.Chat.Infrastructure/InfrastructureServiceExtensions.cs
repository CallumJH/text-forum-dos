using Microsoft.AspNetCore.Identity;
using Whispers.Chat.Core.BoundedContexts.IdentityAndUsers.AggregateRoots;
using Whispers.Chat.Core.Generated.Interfaces;
using Whispers.Chat.Core.Generated.Services;
using Whispers.Chat.Infrastructure.Data;
using Whispers.Chat.Infrastructure.Data.Queries;
using Whispers.Chat.UseCases.Contributors.List;

namespace Whispers.Chat.Infrastructure;

public static class InfrastructureServiceExtensions
{
  public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
  {
    var connectionString = configuration.GetConnectionString("DefaultConnection");

    services.AddDbContext<AppDbContext>(options =>
     options.UseSqlServer(connectionString));

    services.AddIdentity<User, Role>(options =>
    {
      // Password settings
      options.Password.RequireDigit = true;
      options.Password.RequireLowercase = true;
      options.Password.RequireNonAlphanumeric = true;
      options.Password.RequireUppercase = true;
      options.Password.RequiredLength = 8;
      options.Password.RequiredUniqueChars = 1;

      // Lockout settings
      options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
      options.Lockout.MaxFailedAccessAttempts = 5;
      options.Lockout.AllowedForNewUsers = true;

      // User settings
      options.User.RequireUniqueEmail = true;
      options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

    services.ConfigureApplicationCookie(options =>
    {
      options.Cookie.HttpOnly = true;
      options.ExpireTimeSpan = TimeSpan.FromHours(1);
      options.LoginPath = "/Identity/Account/Login";
      options.LogoutPath = "/Identity/Account/Logout";
      options.AccessDeniedPath = "/Identity/Account/AccessDenied";
      options.SlidingExpiration = true;
    });

    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
           .AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>))
           .AddScoped<IListContributorsQueryService, ListContributorsQueryService>()
           .AddScoped<IDeleteContributorService, DeleteContributorService>();

    return services;
  }
}
