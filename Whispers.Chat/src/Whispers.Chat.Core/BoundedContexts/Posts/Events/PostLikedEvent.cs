namespace Whispers.Chat.Core.BoundedContexts.Posts.Events;

public record PostLikedEvent : BaseDomainEvent
{
  public Guid PostId { get; }
  public Guid UserId { get; }

  public PostLikedEvent(Guid postId, Guid userId)
  {
    PostId = postId;
    UserId = userId;
  }
} 
