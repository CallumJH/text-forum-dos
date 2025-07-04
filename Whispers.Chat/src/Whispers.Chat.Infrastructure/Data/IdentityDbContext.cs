using Ardalis.Specification;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Whispers.Chat.Core.BoundedContexts.IdentityAndUsers.Aggregates;

namespace Whispers.Chat.Infrastructure.Data;

public class IdentityContext : IdentityDbContext<User, Role, Guid>
{
  private readonly IDomainEventDispatcher _domainEventDispatcher;

  public IdentityContext(
      DbContextOptions<IdentityContext> options,
      IDomainEventDispatcher domainEventDispatcher) : base(options)
  {
    _domainEventDispatcher = domainEventDispatcher;
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    // Customize the ASP.NET Identity model
    modelBuilder.Entity<User>(b =>
    {
      b.ToTable("Users");
      b.Property(u => u.LikedPostIds)
        .HasConversion(
          v => string.Join(',', v),
          v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(id => Guid.Parse(id))
                .ToList()
        )
        .Metadata.SetValueComparer(new ValueComparer<List<Guid>>(
          (c1, c2) => c1 == null && c2 == null || c1 != null && c2 != null && c1.SequenceEqual(c2),
          c => c == null ? 0 : c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
          c => c == null ? new List<Guid>() : c.ToList()
        ));
    });

    modelBuilder.Entity<Role>(b =>
    {
      b.ToTable("Roles");
    });

    modelBuilder.Entity<IdentityUserRole<Guid>>(b =>
    {
      b.ToTable("UserRoles");
    });

    modelBuilder.Entity<IdentityUserClaim<Guid>>(b =>
    {
      b.ToTable("UserClaims");
    });

    modelBuilder.Entity<IdentityUserLogin<Guid>>(b =>
    {
      b.ToTable("UserLogins");
    });

    modelBuilder.Entity<IdentityRoleClaim<Guid>>(b =>
    {
      b.ToTable("RoleClaims");
    });

    modelBuilder.Entity<IdentityUserToken<Guid>>(b =>
    {
      b.ToTable("UserTokens");
    });
  }
}
