namespace Whispers.Chat.Core.BoundedContexts.Posts.Events;

/// <summary>
/// Event raised when a post is liked by a user
/// </summary>
public class PostLikedEvent : DomainEventBase
{
  public Guid PostId { get; }
  public Guid LikedByUserId { get; }
  public int NewLikeCount { get; }

  public PostLikedEvent(Guid postId, Guid likedByUserId, int newLikeCount)
  {
    PostId = postId;
    LikedByUserId = likedByUserId;
    NewLikeCount = newLikeCount;
  }
} 
