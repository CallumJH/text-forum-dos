using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Whispers.Chat.Core.BoundedContexts.IdentityAndUsers.AggregateRoots;
using Whispers.Chat.Core.BoundedContexts.Posts;
using Whispers.Chat.Infrastructure.Data.Config;

namespace Whispers.Chat.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<User, Role, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options, IDomainEventDispatcher domainEventDispatcher) : base(options)
    {
      
    }

    public DbSet<Post> Posts => Set<Post>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ContributorConfiguration());
        
        // Customize the ASP.NET Identity model
        modelBuilder.Entity<User>(b =>
        {
            b.ToTable("Users");
            b.Property(u => u.LikedPostIds).HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(id => Guid.Parse(id)).ToList()
            );
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
