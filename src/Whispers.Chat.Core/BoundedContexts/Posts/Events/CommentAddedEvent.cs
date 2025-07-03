using System;
using Whispers.Chat.Core.DomainEvents;

namespace Whispers.Chat.Core.BoundedContexts.Posts.Events;

public class CommentAddedEvent : BaseDomainEvent
{
  public Guid PostId { get; }
  public Guid CommentId { get; }
  public Guid AuthorId { get; }
  public string Content { get; }

  public CommentAddedEvent(Guid postId, Guid commentId, Guid authorId, string content)
  {
    PostId = postId;
    CommentId = commentId;
    AuthorId = authorId;
    Content = content;
  }
} 