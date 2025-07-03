namespace Whispers.Chat.Core.BoundedContexts.IdentityAndUsers.Events;
public record UserProfileUpdatedEvent : BaseDomainEvent
{
  public Guid UserId { get; }
  public string Username { get; }
  public string Email { get; }
  public UserProfileUpdatedEvent(Guid userId, string? username, string? email)
  {
    UserId = Guard.Against.NullOrEmpty(userId);
    Username = Guard.Against.NullOrEmpty(username);
    Email = Guard.Against.NullOrEmpty(email);
  }
}
