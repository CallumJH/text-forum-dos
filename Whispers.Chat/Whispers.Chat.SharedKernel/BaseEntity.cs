namespace Whispers.Chat.SharedKernel;

/// <summary>
/// Base class for all entities that use GUID-based IDs
/// </summary>
/// <remarks>
/// This new BaseEntity was created to change the use of int IDs on entities
/// </remarks>
public abstract class BaseEntity()
{
  public string Id { get; protected set; } = Guid.NewGuid().ToString();
  public DateTime DateCreated { get; } = DateTime.Now;
  public DateTime? DateUpdated { get; protected set; }
  public DateTime CreatedBy { get; protected set; }
  public DateTime? UpdatedBy { get; protected set; }
} 
