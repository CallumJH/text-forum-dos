﻿namespace Whispers.Chat.Core.BoundedContexts.IdentityAndUsers.Events;

public record UserRegisteredEvent : BaseDomainEvent
{
  public Guid UserId { get; }
  public string Username { get; }
  public string Email { get; }
  public bool IsAnonymous { get; }

  public UserRegisteredEvent(Guid userId, string? username, string? email, bool isAnonymous)
  {
    UserId = Guard.Against.NullOrEmpty(userId);
    Username = Guard.Against.NullOrEmpty(username);
    Email = Guard.Against.NullOrEmpty(email);
    IsAnonymous = isAnonymous;
  }
} 
