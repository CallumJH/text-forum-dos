namespace Whispers.Chat.Core.BoundedContexts.Posts.Events;

/// <summary>
/// Event raised when a new comment is added to a post
/// </summary>
public class CommentAddedEvent : DomainEventBase
{
  public Guid CommentId { get; }
  public Guid PostId { get; }
  public Guid AuthorId { get; }
  public string Content { get; }

  public CommentAddedEvent(Guid commentId, Guid postId, Guid authorId, string content)
  {
    CommentId = commentId;
    PostId = postId;
    AuthorId = authorId;
    Content = content;
  }
} 
