namespace Whispers.Chat.Core.BoundedContexts.IdentityAndUsers.Events;

/// <summary>
/// Event raised when a new user registers in the system
/// </summary>
public class UserRegisteredEvent : DomainEventBase
{
  public Guid UserId { get; }
  public string Username { get; }
  public string Email { get; }
  public bool IsAnonymous { get; }

  public UserRegisteredEvent(Guid userId, string username, string email, bool isAnonymous)
  {
    UserId = userId;
    Username = username;
    Email = email;
    IsAnonymous = isAnonymous;
  }
} 
