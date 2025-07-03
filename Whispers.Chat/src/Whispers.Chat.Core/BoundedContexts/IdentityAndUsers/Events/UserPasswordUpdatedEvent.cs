namespace Whispers.Chat.Core.BoundedContexts.IdentityAndUsers.Events;
public record UserPasswordUpdatedEvent : BaseDomainEvent
{
  public Guid UserId { get; }
  public UserPasswordUpdatedEvent(Guid userId)
  { 
    UserId = Guard.Against.NullOrEmpty(userId);
  }
}
