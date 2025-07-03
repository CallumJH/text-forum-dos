namespace Whispers.Chat.Core.BoundedContexts.IdentityAndUsers.Events;
public record UserLoggedInEvent : BaseDomainEvent
{
  public Guid UserId { get; }
  public string Username { get; }

  public UserLoggedInEvent(Guid userId, string? username)
  {
    UserId = Guard.Against.NullOrEmpty(userId);
    Username = Guard.Against.NullOrEmpty(username);
  }
}
