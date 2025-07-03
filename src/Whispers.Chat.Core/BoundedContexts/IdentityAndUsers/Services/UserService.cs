using System;
using System.Threading.Tasks;
using Whispers.Chat.Core.BoundedContexts.IdentityAndUsers.AggregateRoots;
using Whispers.Chat.Core.BoundedContexts.IdentityAndUsers.Events;
using Whispers.Chat.Core.Interfaces;

namespace Whispers.Chat.Core.BoundedContexts.IdentityAndUsers.Services;

public class UserService : IDomainService
{
  public async Task<User> RegisterUser(string username, string email)
  {
    var user = new User(username, email);
    user.AddDomainEvent(new UserRegisteredEvent(user.Id, username, email));
    return user;
  }

  public async Task UpdateUserProfile(User user, string username = null, string email = null)
  {
    if (username != null)
    {
      user.UpdateUsername(username);
    }

    if (email != null)
    {
      user.UpdateEmail(email);
    }
  }
} 