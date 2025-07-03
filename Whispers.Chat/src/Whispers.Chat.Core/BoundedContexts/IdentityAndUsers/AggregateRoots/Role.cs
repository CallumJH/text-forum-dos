using Microsoft.AspNetCore.Identity;
using Ardalis.SharedKernel;

namespace Whispers.Chat.Core.BoundedContexts.IdentityAndUsers.AggregateRoots;

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