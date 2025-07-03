using System.Text.Json;
using Whispers.Chat.Core.BoundedContexts.IdentityAndUsers.Aggregates;
using Whispers.Chat.Core.BoundedContexts.SiteModeration.Aggregates;

namespace Whispers.Chat.Infrastructure.Persistence.Configurations;

public class ModeratorConfiguration : IEntityTypeConfiguration<Moderator>
{
  public void Configure(EntityTypeBuilder<Moderator> builder)
  {
    // Table name
    builder.ToTable("Moderators");

    // Primary key
    builder.HasKey(m => m.Id);

    // Properties configuration
    builder.Property(m => m.UserId)
        .IsRequired();

    builder.Property(m => m.IsActive)
        .IsRequired()
        .HasDefaultValue(true);

    // Configure ModeratedPostIds as JSON (using backing field)
    builder.Property<List<Guid>>("_moderatedPostIds")
        .HasConversion(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => JsonSerializer.Deserialize<List<Guid>>(v, (JsonSerializerOptions?)null) ?? new List<Guid>())
        .HasColumnName("ModeratedPostIds")
        .HasColumnType("nvarchar(max)")
        .IsRequired();

    // Audit properties
    builder.Property(m => m.DateCreated)
        .IsRequired()
        .HasDefaultValueSql("GETUTCDATE()");

    builder.Property(m => m.DateUpdated)
        .IsRequired(false);

    builder.Property(m => m.CreatedBy)
        .IsRequired();

    builder.Property(m => m.UpdatedBy)
        .IsRequired(false);

    // Ignore computed properties (read-only collection)
    builder.Ignore(m => m.ModeratedPostIds);

    // Relationships
    builder.HasOne<User>()
        .WithMany()
        .HasForeignKey(m => m.UserId)
        .OnDelete(DeleteBehavior.Restrict);

    // Indexes
    builder.HasIndex(m => m.UserId)
        .IsUnique() // One moderator record per user
        .HasDatabaseName("IX_Moderators_UserId");

    builder.HasIndex(m => m.IsActive)
        .HasDatabaseName("IX_Moderators_IsActive");

    builder.HasIndex(m => m.DateCreated)
        .HasDatabaseName("IX_Moderators_DateCreated");

    // Composite index for active moderators
    builder.HasIndex(m => new { m.IsActive, m.DateCreated })
        .HasDatabaseName("IX_Moderators_IsActive_DateCreated");

    // Configure private setters
    builder.Property(m => m.UserId).UsePropertyAccessMode(PropertyAccessMode.Field);
    builder.Property(m => m.IsActive).UsePropertyAccessMode(PropertyAccessMode.Field);
    builder.Property(m => m.DateUpdated).UsePropertyAccessMode(PropertyAccessMode.Field);
    builder.Property(m => m.UpdatedBy).UsePropertyAccessMode(PropertyAccessMode.Field);
  }
}
