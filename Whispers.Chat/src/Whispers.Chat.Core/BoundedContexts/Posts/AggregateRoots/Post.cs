namespace Whispers.Chat.Core.BoundedContexts.Posts;

public class Post : EntityBase<Guid>, IAggregateRoot, IAuditableEntity
{
  private readonly List<Comment> _comments = new();
  private readonly List<Guid> _likedByUserIds = new();
  public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();
  public IReadOnlyCollection<Guid> LikedByUserIds => _likedByUserIds.AsReadOnly();

  public string Title { get; private set; } = string.Empty;
  public string Content { get; private set; } = string.Empty;
  public Guid AuthorId { get; private set; }
  public int LikeCount => _likedByUserIds.Count;
  public bool IsMisleading { get; private set; }
  public bool IsFalseInformation { get; private set; }
  public List<string> Tags { get; private set; } = new();

  public DateTime DateCreated { get; private init; } = DateTime.UtcNow;
  public DateTime? DateUpdated { get; private set; }
  public Guid CreatedBy { get; private init; }
  public Guid? UpdatedBy { get; private set; }

  #region EFCore constructor
  protected Post()
  {
    Id = Guid.NewGuid();
  }
  #endregion

  public Post(string title, string content, Guid authorId, Guid createdBy) : this()
  {
    Title = Guard.Against.NullOrEmpty(title, nameof(title));
    Content = Guard.Against.NullOrEmpty(content, nameof(content));
    AuthorId = authorId;
    CreatedBy = createdBy;
  }

  public Comment AddComment(string content, Guid authorId, Guid updatedBy)
  {
    var comment = new Comment(content, authorId, Id, updatedBy);
    _comments.Add(comment);
    DateUpdated = DateTime.UtcNow;
    UpdatedBy = updatedBy;
    return comment;
  }

  public void RemoveComment(Guid commentId, Guid updatedBy)
  {
    var comment = _comments.FirstOrDefault(c => c.Id == commentId);
    if (comment != null)
    {
      DateUpdated = DateTime.UtcNow;
      UpdatedBy = updatedBy;
      _comments.Remove(comment);
    }
  }

  public void AddLike(Guid userId, Guid updatedBy)
  {
    if (userId == AuthorId)
    {
      throw new DomainException("Authors cannot like their own posts");
    }

    if (_likedByUserIds.Contains(userId))
    {
      throw new DomainException("User has already liked this post");
    }

    _likedByUserIds.Add(userId);
    DateUpdated = DateTime.UtcNow;
    UpdatedBy = updatedBy;
  }

  public void MarkAsMisleading(Guid updatedBy)
  {
    IsMisleading = true;
    DateUpdated = DateTime.UtcNow;
    UpdatedBy = updatedBy;
  }

  public void MarkAsFalseInformation(Guid updatedBy)
  {
    IsFalseInformation = true;
    DateUpdated = DateTime.UtcNow;
    UpdatedBy = updatedBy;
  }

  public void ClearModerationFlags(Guid updatedBy)
  {
    IsMisleading = false;
    IsFalseInformation = false;
    DateUpdated = DateTime.UtcNow;
    UpdatedBy = updatedBy;
  }

  public void AddTag(string tag, Guid updatedBy)
  {
    if (!Tags.Contains(tag))
    {
      Tags.Add(tag);
      DateUpdated = DateTime.UtcNow;
      UpdatedBy = updatedBy;
    }
  }

  public void RemoveTag(string tag, Guid updatedBy)
  {
    if (Tags.Remove(tag))
    {
      DateUpdated = DateTime.UtcNow;
      UpdatedBy = updatedBy;
    }
  }

  public void UpdateContent(string newTitle, string newContent, Guid updatedBy)
  {
    Title = Guard.Against.NullOrEmpty(newTitle, nameof(newTitle));
    Content = Guard.Against.NullOrEmpty(newContent, nameof(newContent));
    DateUpdated = DateTime.UtcNow;
    UpdatedBy = updatedBy;
  }
}
