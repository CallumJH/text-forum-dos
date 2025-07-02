namespace Whispers.Chat.Core.BoundedContexts.Posts;

public class Post : BaseEntity, IAggregateRoot
{
  private readonly List<Comment> _comments = new();
  public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();

  public string Title { get; private set; } = string.Empty;
  public string Content { get; private set; } = string.Empty;
  public string AuthorId { get; private set; } = string.Empty;

  #region EFCore constructor
  protected Post()
  {
  }
  #endregion

  public Post(string title, string content, string authorId) : this()
  {
    Title = Guard.Against.NullOrEmpty(title, nameof(title));
    Content = Guard.Against.NullOrEmpty(content, nameof(content));
    AuthorId = Guard.Against.NullOrEmpty(authorId, nameof(authorId));
  }

  public Comment AddComment(string content, string authorId)
  {
    var comment = new Comment(content, authorId, Id);
    _comments.Add(comment);
    return comment;
  }

  public void RemoveComment(string commentId)
  {
    var comment = _comments.FirstOrDefault(c => c.Id == commentId);
    if (comment != null)
    {
      DateUpdated = DateTime.UtcNow;
      _comments.Remove(comment);
    }
  }

  public void UpdateContent(string newTitle, string newContent)
  {
    Title = Guard.Against.NullOrEmpty(newTitle, nameof(newTitle));
    Content = Guard.Against.NullOrEmpty(newContent, nameof(newContent));
    DateUpdated = DateTime.UtcNow;
  }
}
