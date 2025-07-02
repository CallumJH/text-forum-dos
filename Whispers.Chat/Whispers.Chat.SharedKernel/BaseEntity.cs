namespace Whispers.Chat.SharedKernel;

/// <summary>
/// Interface for entities that need audit information
/// </summary>
public interface IAuditableEntity
{
  DateTime DateCreated { get; }
  DateTime? DateUpdated { get; }
  Guid CreatedBy { get; }
  Guid? UpdatedBy { get; }
}
