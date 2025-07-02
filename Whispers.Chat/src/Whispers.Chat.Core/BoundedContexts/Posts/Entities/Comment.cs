namespace Whispers.Chat.Core.BoundedContexts.Posts;

public class Comment : BaseEntity
{
  public string Content { get; private set; } = string.Empty;
  public string AuthorId { get; private set; } = string.Empty;
  public string PostId { get; private set; } = string.Empty;

  #region EFCore constructor
  protected Comment()
  {
  }
  #endregion

  public Comment(string content, string authorId, string postId) : this()
  {
    Content = Guard.Against.NullOrEmpty(content, nameof(content));
    AuthorId = Guard.Against.NullOrEmpty(authorId, nameof(authorId));
    PostId = Guard.Against.NullOrEmpty(postId, nameof(postId));
  }

  public void UpdateContent(string newContent)
  {
    Content = Guard.Against.NullOrEmpty(newContent, nameof(newContent));
    DateUpdated = DateTime.UtcNow;
  }
}
