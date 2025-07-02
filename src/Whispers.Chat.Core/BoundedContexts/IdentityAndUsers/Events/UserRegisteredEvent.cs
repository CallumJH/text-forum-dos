using System;
using Whispers.Chat.Core.DomainEvents;

namespace Whispers.Chat.Core.BoundedContexts.IdentityAndUsers.Events;

public class UserRegisteredEvent : BaseDomainEvent
{
  public Guid UserId { get; }
  public string Username { get; }
  public string Email { get; }

  public UserRegisteredEvent(Guid userId, string username, string email)
  {
    UserId = userId;
    Username = username;
    Email = email;
  }
} 