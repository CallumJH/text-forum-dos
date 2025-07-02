using System;
using System.Collections.Generic;
using System.Linq;
using Whispers.Chat.Core.BoundedContexts.Posts.Entities;
using Whispers.Chat.Core.BoundedContexts.Posts.Events;
using Whispers.Chat.SharedKernel;

namespace Whispers.Chat.Core.BoundedContexts.Posts.AggregateRoots;

public class Post : BaseEntity, IAuditableEntity
{
  public string Content { get; private set; }
  public Guid AuthorId { get; private set; }
  private readonly List<Guid> _likes = new();
  private readonly List<Comment> _comments = new();
  public IReadOnlyCollection<Guid> Likes => _likes.AsReadOnly();
  public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();

  public DateTime DateCreated { get; private set; }
  public DateTime? DateUpdated { get; private set; }
  public Guid CreatedBy { get; private set; }
  public Guid? UpdatedBy { get; private set; }

  public Post(string content, Guid authorId)
  {
    Content = content;
    AuthorId = authorId;
    Id = Guid.NewGuid();
    DateCreated = DateTime.UtcNow;
    CreatedBy = authorId;

    AddDomainEvent(new PostCreatedEvent(Id, authorId, content));
  }

  public void AddLike(Guid userId)
  {
    if (!HasUserLiked(userId))
    {
      _likes.Add(userId);
      AddDomainEvent(new PostLikedEvent(Id, userId));
    }
  }

  public bool HasUserLiked(Guid userId)
  {
    return _likes.Contains(userId);
  }

  public void AddComment(string content, Guid userId)
  {
    var comment = new Comment(content, userId);
    _comments.Add(comment);
    AddDomainEvent(new CommentAddedEvent(Id, comment.Id, userId, content));
  }
} 