using Microsoft.AspNetCore.Identity;

namespace Whispers.Chat.Core.BoundedContexts.IdentityAndUsers.Aggregates;

public class Role : IdentityRole<Guid>, IAggregateRoot
{
    public Role() : base()
    {
        Id = Guid.NewGuid();
    }

    public Role(string roleName) : base(roleName)
    {
        Id = Guid.NewGuid();
    }
} 
