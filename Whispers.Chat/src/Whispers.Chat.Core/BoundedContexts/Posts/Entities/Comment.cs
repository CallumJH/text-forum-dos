using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Whispers.Chat.SharedKernel;

namespace Whispers.Chat.Core.BoundedContexts.Posts;

public class Comment : EntityBase<Guid>, IAuditableEntity
{
  public string Content { get; private set; } = string.Empty;
  public Guid AuthorId { get; private set; }
  public Guid PostId { get; private set; }

  public DateTime DateCreated { get; private init; } = DateTime.UtcNow;
  public DateTime? DateUpdated { get; private set; }
  public Guid CreatedBy { get; private init; }
  public Guid? UpdatedBy { get; private set; }

  #region EFCore constructor
  protected Comment()
  {
    Id = Guid.NewGuid();
  }
  #endregion

  public Comment(string content, Guid authorId, Guid postId, Guid createdBy) : this()
  {
    Content = Guard.Against.NullOrEmpty(content, nameof(content));
    AuthorId = authorId;
    PostId = postId;
    CreatedBy = createdBy;
  }

  public void UpdateContent(string newContent, Guid updatedBy)
  {
    Content = Guard.Against.NullOrEmpty(newContent, nameof(newContent));
    DateUpdated = DateTime.UtcNow;
    UpdatedBy = updatedBy;
  }
}
