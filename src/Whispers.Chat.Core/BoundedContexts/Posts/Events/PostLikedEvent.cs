using System;
using Whispers.Chat.Core.DomainEvents;

namespace Whispers.Chat.Core.BoundedContexts.Posts.Events;

public class PostLikedEvent : BaseDomainEvent
{
  public Guid PostId { get; }
  public Guid UserId { get; }

  public PostLikedEvent(Guid postId, Guid userId)
  {
    PostId = postId;
    UserId = userId;
  }
} 